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

        public Controller getController() {
            return controller;
        }

        public virtual bool isTouchedWall() {
            // check character is in front of wall
            CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
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
