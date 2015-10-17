using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Character : ObjectBase {
        // prefabs
        public GameObject status_prefab;

        // references to component
        protected Controller controller;

        // vars
        protected FrameTimerState inputlock_timer;
        protected CharacterStatus status;
        protected RingBuffer<Vector3> position_history;

        public Controller getController() {
            return controller;
        }

        public virtual bool isTouchedWall() {
            // check character is in front of wall
            //CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
            Collider capsule_collider = GetComponent<Collider>();
            return Physics.Raycast(transform.position + new Vector3(0, capsule_collider.bounds.size.y, 0), transform.forward, 0.4f) ||
                Physics.Raycast(capsule_collider.bounds.center, transform.forward, 0.4f);
        }

        public int getHP() {
            return status.hp;
        }

        public void setHP(int _hp) {
            status.hp = _hp;
        }

        public bool isFrozen() {
            return status.freeze;
        }

        public virtual void damage(int dam) {
            if ( !status.invincible.now() ) {
                status.invincible.enable(30);
                status.hp -= dam;
            }
        }

        protected void recordPosition() {
            position_history.add(transform.position);
        }

        public int getPositionHistoryCount() {
            return position_history.count();
        }

        public void clearPositionHistory() {
            position_history.clear();
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
                index++;
                prev = pos;
            }
            return travel;
        }


        // xdir = 1.0f | -1.0f
        public void lookAtDirectionX(float xdir) {
            transform.LookAt(
                new Vector3(
                    transform.position.x + xdir * 100.0f, 
                    transform.position.y, 
                    transform.position.z
                )
            );
        }

        public void lockInput(int frame) {
            // disable movement by inputs for N frames specified
            inputlock_timer.destroy();
            inputlock_timer.createTimer(frame);
        }

        public bool isInputLocked() {
            // return true when inputs are locked
            return inputlock_timer.isRunning();
        }


    }
}
