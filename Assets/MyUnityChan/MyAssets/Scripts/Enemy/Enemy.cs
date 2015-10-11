using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class Enemy : NPCharacter {
        public GameObject controller_prefab;
        public GameObject enemy_action_manager_prefab;

        protected int max_hp;

        protected int stunned = 0;
        protected RingBuffer<Vector3> position_history;
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
            }
        }

        public virtual void spawn() {
            touching_players.Clear();
            setHP(max_hp);
            this.gameObject.SetActive(true);
        }

        protected void recordPosition() {
            position_history.add(transform.position);
        }

        public Vector3 getRecentTravelDistance() {
            Vector3 travel = Vector3.zero;
            Vector3 prev = Vector3.zero;
            int index = 0;
            foreach ( Vector3 pos in position_history ) {
                if ( index == 0 ) {
                    prev = pos;
                    index++;
                    continue;
                }
                travel = travel + new Vector3(Mathf.Abs(prev.x - pos.x), Mathf.Abs(prev.y - pos.y), 0.0f);
            }
            return travel;
        }


        // Use this for initialization
        void Start() {
            loadAttachedAI();
            action_manager = new EnemyActionManager();
            inputlock_timer = new FrameTimerState();
            position_history = new RingBuffer<Vector3>(6);

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
