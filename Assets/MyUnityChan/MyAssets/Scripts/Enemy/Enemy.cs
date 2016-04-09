using UnityEngine;
using System.Collections.Generic;
using EnergyBarToolkit;

namespace MyUnityChan {

    public abstract class Enemy : NPCharacter {
        public string AI_name;

        protected int max_hp;
        protected EnemyActionManager action_manager;

        public HpGauge hp_gauge { get; protected set; }

        protected void loadAttachedAI() {
            GameObject controller_inst = PrefabInstantiater.create(Const.Prefab.AI[AI_name], gameObject);
            controller = controller_inst.GetComponent<Controller>();

            ((AIController)controller).setSelf(this);
        }

        public virtual void spawn() {
            touching_players.Clear();
            setHP(max_hp);
            clearPositionHistory();
            this.gameObject.SetActive(true);
        }

        void Awake() {
            action_manager = GetComponent<EnemyActionManager>();
            hp_gauge = null;
        }

        // Use this for initialization
        void Start() {
            loadAttachedAI();
            inputlock_timer = new FrameTimerState();
            position_history = new RingBuffer<Vector3>(10);

            // enemy status setup
            status = GetComponent<EnemyStatus>();

            start();
        }

        // Update is called once per frame
        void Update() {
            if ( PauseManager.isPausing() ) return;

            if ( SettingManager.Instance.get(Settings.Flag.ENEMY_STOP) ) {
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

            update();
        }

        public override void damage(int dam) {
            base.damage(dam);

            if ( hp_gauge == null ) {
                hp_gauge = PrefabInstantiater.create(
                    Const.Prefab.UI["ENEMY_HP_GAUGE"], 
                    GUIObjectBase.getCanvas("Canvas_WorldSpace")).GetComponent<HpGauge>();
                hp_gauge.setCharacter(this);
                hp_gauge.gameObject.transform.position = gameObject.transform.position.add(0, 2.0f, 0);
            }
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
