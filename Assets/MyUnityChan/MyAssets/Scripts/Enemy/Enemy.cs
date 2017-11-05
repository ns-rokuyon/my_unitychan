using UnityEngine;
using System.Collections.Generic;
using EnergyBarToolkit;
using UniRx;
using UniRx.Triggers;
using System.Collections;

namespace MyUnityChan {

    [RequireComponent(typeof(EnemyActionManager))]
    [RequireComponent(typeof(EnemyStatus))]
    [RequireComponent(typeof(GroundChecker))]
    public abstract class Enemy : NPCharacter {
        /*
            Tag: Enemy
            Layer: Character
        */
        public Const.ID.Enemy enemy_id;
        public Const.ID.EnemyFamily enemyfamily_id;
        public Const.ID.Effect dead_effect;
        public int max_hp;

        protected CharacterStatus _status;
        public override CharacterStatus status {
            get {
                return _status ?? (_status = GetComponent<EnemyStatus>());
            }
            set {
                _status = value;
            }
        }
        public EnemyActionManager action_manager { get; protected set; }
        public HpGauge hp_gauge { get; protected set; }

        public int level {
            get {
                return levelInFamily();
            }
        }  // >= 1

        [SerializeField, ReadOnly]
        public int _exp;
        public int exp {
            get { return _exp; }
            set { _exp = value; }
        }

        public override void OnEnable() {
            base.OnEnable();
            if ( controller )
                ((AIController)controller).restart();
        }

        protected void loadAttachedAI() {
            GameObject controller_inst = PrefabInstantiater.create(prefabPath(Const.ID.Controller.AI), gameObject);
            controller_inst.transform.localPosition = Vector3.zero;
            controller = controller_inst.GetComponent<Controller>();

            ((AIController)controller).setSelf(this);
        }

        public override bool assert() {
            return base.assert() && Const.EnemyFamily[enemyfamily_id].Contains(enemy_id);
        }

        public virtual void spawn() {
            touching_players.Clear();
            setHP(max_hp);
            clearPositionHistory();
            if ( controller ) {
                Destroy(controller.gameObject);
            }
            this.gameObject.SetActive(true);
            loadAttachedAI();
        }

        // Awake
        protected override void awake() {
            if ( !assert() ) {
                throw new System.FormatException("Assertion Error! : " + this.name);
            }
            action_manager = GetComponent<EnemyActionManager>();
            hp_gauge = null;
            exp = 0;
            setupSoundPlayer();

            // enemy status setup
            status = GetComponent<EnemyStatus>();
        }

        // Start
        protected override void start() {
            loadAttachedAI();
            position_history = new RingBuffer<Vector3>(10);

            this.ObserveEveryValueChanged(_ => stunned)
                .Where(st => st == 0)
                .Subscribe(_ => {
                    var anim = GetComponent<Animator>();
                    if ( anim )
                        anim.speed = 1.0f;
                }).AddTo(gameObject);
         }

        // Update is called once per frame
        protected override void update() {
            if ( time_control.paused ) return;
            if ( !controller ) return;

            if ( SettingManager.get(Settings.Flag.ENEMY_STOP) ) {
                (controller as AIController).isStopped = true;
            }
            else {
                (controller as AIController).isStopped = false;
            }
            updateStunned();
            checkPlayerTouched();
            faceForward();
            recordPosition();
            followHpGauge();
            leveling();
        }

        public override void stun(int stun_power) {
            base.stun(stun_power);
            if ( this is IEnemyStun )
                (this as IEnemyStun).onStun();
        }

        public override void damage(int dam) {
            dam = (int)(dam * 
                GameStateManager.getPlayer().manager.status.setting.ranges[Settings.Range.PLAYER_POWER_SCALE].value);

            base.damage(dam);

            if ( this is IEnemyTakeDamage ) {
                (this as IEnemyTakeDamage).takeDamage(dam);
            }

            if ( hp_gauge == null ) {
                if ( !SettingManager.get(Settings.Flag.SHOW_ENEMY_HP_BAR) )
                    return;

                hp_gauge = PrefabInstantiater.createWorldUIAndGetComponent<HpGauge>(
                    Const.Prefab.UI["ENEMY_HP_GAUGE"], Vector3.one * 0.2f);
                hp_gauge.setCharacter(this);
                hp_gauge.setMapHp(max_hp);
                hp_gauge.gameObject.transform.position = gameObject.transform.position.add(0, height, 0);
            }

            EffectManager.createTextEffect(dam.ToString(),
                                           Const.Prefab.Effect[Const.ID.Effect.DAMAGE_INDICATOR],
                                           transform.position.add(0, height, 0), 60, true);
        }

        public override void knockback(int dam) {
            base.knockback(dam);
            if ( !(this is IEnemyKnockback) )
                return;

            var kb = this as IEnemyKnockback;
            if ( dam >= kb.getKnockbackThreshold() ) {
                kb.onKnockback();
            }
        }

        public void leveling() {
            if ( this is IEnemyLevelUp ) {
                if ( getMaxLevel() == level ) {
                    // Max level
                    return;
                }
                var enemy = this as IEnemyLevelUp;
                if ( exp >= enemy.getMaxExp() ) {
                    Vector3 effect_position;
                    if ( ground_checker && ground_checker.isGrounded() )
                        effect_position = ground_checker.point();
                    else
                        effect_position = transform.position;
                    EffectManager.createEffect(Const.ID.Effect.ENEMY_LEVEL_UP, effect_position, 60, true);

                    exp = 0;
                    createNextLevelEnemy();
                    deactivate(destroy: true);
                }
            }
        }

        protected virtual void createNextLevelEnemy() {
            string create_to = transform.parent.gameObject.getHierarchyPath();
            string next_enemy_prefab_path = Const.Prefab.Enemy[getEnemyIdNextLevel()];
            Vector3 last_position = transform.position;

            GameStateManager.self().delay(7, () => {
                GameObject obj = PrefabInstantiater.create(next_enemy_prefab_path, create_to);
                obj.transform.position = last_position;
                AreaManager.self().relabelObject(obj);
            });
        }

        public void deactivate(bool destroy = false) {
            setHP(0);
            destroyHpGauge();
            gameObject.SetActive(false);
            delay(5, () => Destroy(gameObject));
        }

        public int getMaxLevel() {
            return Const.EnemyFamily[enemyfamily_id].Count;
        }

        public bool amI(Const.ID.Enemy eid) {
            return enemy_id == eid;
        }

        public int levelInFamily() {
            return Const.EnemyFamily[enemyfamily_id].IndexOf(enemy_id) + 1;
        }

        public Const.ID.Enemy getEnemyIdNextLevel() {
            return Const.EnemyFamily[enemyfamily_id][level];
        }

        public override void defeatSomeone(Character character) {
            int defeat_id = character.gameObject.GetInstanceID();
            if ( defeat_records.Contains(defeat_id) )
                return;

            exp += Const.Unit.EXP_ENEMY_DEFEATS_SOMEONE;
            defeat_records.Add(defeat_id);
        }

        public void followHpGauge() {
            if ( hp_gauge ) {
                hp_gauge.gameObject.transform.position = gameObject.transform.position.add(0, height , 0);
            }
        }

        public void destroyHpGauge() {
            if ( hp_gauge ) {
                Destroy(hp_gauge.gameObject);
                hp_gauge = null;
            }
        }

    }
}
