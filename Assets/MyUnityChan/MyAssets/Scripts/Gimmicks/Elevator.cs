using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Elevator : MovingFloor {
        public enum State {
            GOING_UP,
            GOING_DOWN,
            WAITING_TOP,
            WAITING_BOTTOM
        }

        public Vector3 top;
        public Vector3 bottom;
        public float speed;

        public Elevator.State state { get; private set; }

        // Use this for initialization
        void Start() {
            // Initialize state
            if ( Vector3.Distance(top, transform.position) > Vector3.Distance(bottom, transform.position) ) {
                state = State.WAITING_BOTTOM;
            }
            else {
                state = State.WAITING_TOP;
            }
        }

        // Update is called once per frame
        void Update() {
            if ( state == State.GOING_UP ) {
                // Move
                transform.localPosition = transform.localPosition.add(0, speed * Time.deltaTime, 0);
                if ( transform.localPosition.y > top.y ) {
                    // Stop
                    transform.localPosition = top;
                    state = State.WAITING_TOP;
                    freePlayer();
                }
            }
            else if ( state == State.GOING_DOWN ) {
                // Move
                transform.localPosition = transform.localPosition.add(0, -speed * Time.deltaTime, 0);
                if ( transform.localPosition.y < bottom.y ) {
                    // Stop
                    transform.localPosition = bottom;
                    state = State.WAITING_BOTTOM;
                    freePlayer();
                }
            }
            else {
                PlayerManager pm = getPlayerManager();
                if ( pm ) {
                    if ( pm.controller.keyUp() && state == State.WAITING_BOTTOM ) {
                        // Start
                        state = State.GOING_UP;
                        lockPlayer();
                    }
                    else if ( pm.controller.keyDown() && state == State.WAITING_TOP ) {
                        // Start
                        state = State.GOING_DOWN;
                        lockPlayer();
                    }
                }
            }
        }

        protected void lockPlayer() {
            PlayerManager pm = getPlayerManager();
            if ( pm )
                pm.getNowPlayerComponent().freeze();
        }

        protected void freePlayer() {
            PlayerManager pm = getPlayerManager();
            if ( pm )
                pm.getNowPlayerComponent().freeze(false);
        }
    }

}
