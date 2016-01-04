using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class Enemy : NPCharacter {
        public string AI_name;

        protected int max_hp;
        protected EnemyActionManager action_manager;

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
        }

        // Use this for initialization
        void Start() {
            loadAttachedAI();
            inputlock_timer = new FrameTimerState();
            position_history = new RingBuffer<Vector3>(10);

            // enemy status setup
            status = PrefabInstantiater.create(Const.Prefab.Status["ENEMY_STATUS"], gameObject).GetComponent<EnemyStatus>();

            start();
        }

        // Update is called once per frame
        void Update() {
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

            update();
        }


    }
}
