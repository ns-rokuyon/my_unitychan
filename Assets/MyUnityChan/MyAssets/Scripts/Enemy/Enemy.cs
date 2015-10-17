using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class Enemy : NPCharacter {
        public GameObject controller_prefab;
        public GameObject enemy_action_manager_prefab;

        protected int max_hp;

        protected int stunned = 0;
        protected EnemyActionManager action_manager;

        protected void loadAttachedAI() {

            GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
            controller_inst.setParent(gameObject);
            controller = controller_inst.GetComponent<Controller>();

            ((AIController)controller).setSelf(this);
        }

        public void stun(int stun_power) {
            stunned = stun_power;
        }


        public bool isStunned() {
            return stunned > 0 ? true : false;
        }

        protected void updateStunned() {
            if ( stunned > 0 ) {
                stunned--;
                clearPositionHistory();
            }
        }

        public virtual void spawn() {
            touching_players.Clear();
            setHP(max_hp);
            clearPositionHistory();
            this.gameObject.SetActive(true);
        }


        // Use this for initialization
        void Start() {
            loadAttachedAI();
            action_manager = new EnemyActionManager();
            inputlock_timer = new FrameTimerState();
            position_history = new RingBuffer<Vector3>(10);

            // enemy status setup
            status = (Instantiate(status_prefab) as GameObject).setParent(gameObject).GetComponent<EnemyStatus>();

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
