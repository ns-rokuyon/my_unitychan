using UnityEngine;
using System.Collections.Generic;
using EnergyBarToolkit;
using UniRx;

namespace MyUnityChan {

    [RequireComponent(typeof(EnemyActionManager))]
    [RequireComponent(typeof(EnemyStatus))]
    [RequireComponent(typeof(GroundChecker))]
    public abstract class Enemy : NPCharacter {
        /*
            Tag: Enemy
            Layer: Character
        */
        public Const.ID.AI AI_name;
        public Const.ID.Enemy enemy_id;
        public Const.ID.EnemyFamily enemyfamily_id;
        public int max_hp;

        protected EnemyActionManager action_manager;

        public HpGauge hp_gauge { get; protected set; }

        private int __level;
        public int level {
            get {
                if ( Application.isPlaying ) {
                    return this.__level;
                } else {
                    return levelInFamily();
                }
            }
            set {
                this.__level = value;
            }
        }  // >= 1
        public int exp { get; set; }

        protected void loadAttachedAI() {
            GameObject controller_inst = PrefabInstantiater.create(prefabPath(AI_name), gameObject);
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
            this.gameObject.SetActive(true);
        }

        // Awake
        protected override void awake() {
            if ( !assert() ) {
                throw new System.FormatException("Assertion Error! : " + this.name);
            }
            action_manager = GetComponent<EnemyActionManager>();
            hp_gauge = null;
            level = levelInFamily();
            exp = 0;
        }

        // Start
        protected override void start() {
            loadAttachedAI();
            inputlock_timer = new FrameTimerState();
            position_history = new RingBuffer<Vector3>(10);

            // enemy status setup
            status = GetComponent<EnemyStatus>();

            if ( this is IEnemyLevelUp )
                (this as IEnemyLevelUp).levelUp();  // Apply initialized level

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
            if ( PauseManager.isPausing() ) return;

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

            Animator animator = GetComponent<Animator>();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if ( rigidbody )
                rigidbody.velocity = Vector3.zero;
            if ( animator )
                animator.speed = 0.2f;
        }

        public override void damage(int dam) {
            base.damage(dam);

            if ( hp_gauge == null ) {
                if ( !SettingManager.get(Settings.Flag.SHOW_ENEMY_HP_BAR) )
                    return;

                hp_gauge = PrefabInstantiater.create(
                    Const.Prefab.UI["ENEMY_HP_GAUGE"], 
                    GUIObjectBase.getCanvas("Canvas_WorldSpace")).GetComponent<HpGauge>();
                hp_gauge.setCharacter(this);
                hp_gauge.setMapHp(max_hp);
                hp_gauge.gameObject.transform.position = gameObject.transform.position.add(0, 2.0f, 0);
            }
        }

        public void leveling() {
            if ( this is IEnemyLevelUp ) {
                if ( Const.EnemyFamily[enemyfamily_id].Count == level ) {
                    // Max level
                    return;
                }
                var enemy = this as IEnemyLevelUp;
                if ( exp >= enemy.getMaxExp() ) {
                    exp = 0;
                    // Updating level
                    level++;
                    enemy_id = Const.EnemyFamily[enemyfamily_id][level - 1];
                    enemy.levelUp();
                    setHP(max_hp);
                }
            }
        }

        public int getMaxLevel() {
            return Const.EnemyFamily[enemyfamily_id].Count;
        }

        public int levelInFamily() {
            return Const.EnemyFamily[enemyfamily_id].IndexOf(enemy_id) + 1;
        }

        public override void defeatSomeone(Character character) {
            exp += Const.Unit.EXP_ENEMY_DEFEATS_SOMEONE;
        }

        public void followHpGauge() {
            if ( hp_gauge ) {
                hp_gauge.gameObject.transform.position = gameObject.transform.position.add(0, 2.0f, 0);
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
