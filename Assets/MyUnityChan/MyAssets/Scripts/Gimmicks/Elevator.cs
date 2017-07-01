using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

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

            var zone = GetComponentInChildren<SensorZone>();
            if ( zone ) {
                zone.onPlayerStayCallback = (Player player, Collider collider) => {
                    if ( isMember(player.manager) )
                        return;

                    if ( state == State.WAITING_BOTTOM && player.getController().keyUp() ) {
                        // Start
                        getOn(player);
                        lockMembers();
                        state = State.GOING_UP;
                    }
                    else if ( state == State.WAITING_TOP && player.getController().keyDown() ) {
                        // Start
                        getOn(player);
                        lockMembers();
                        state = State.GOING_DOWN;
                    }
                };
            }
            else {
                DebugManager.warn("SensorZone component is not in children. This elevetor cannot move");
            }

            this.UpdateAsObservable()
                .Where(_ => state == State.GOING_UP)
                .Subscribe(_ => {
                    // Move
                    transform.localPosition = transform.localPosition.add(0, speed * Time.deltaTime, 0);
                    if ( transform.localPosition.y > top.y ) {
                        // Stop
                        transform.localPosition = top;
                        freeMembers();
                        state = State.WAITING_TOP;
                    }
                }).AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => state == State.GOING_DOWN)
                .Subscribe(_ => {
                    // Move
                    transform.localPosition = transform.localPosition.add(0, -speed * Time.deltaTime, 0);
                    if ( transform.localPosition.y < bottom.y ) {
                        // Stop
                        transform.localPosition = bottom;
                        state = State.WAITING_BOTTOM;
                        freeMembers();
                    }
                }).AddTo(this);
        }

        protected void lockMembers() {
            members.ForEach(m => {
                if ( m is PlayerManager ) {
                    (m as PlayerManager).getNowPlayerComponent().freeze();
                }
            });
        }

        protected void freeMembers() {
            members.ForEach(m => {
                if ( m is PlayerManager ) {
                    (m as PlayerManager).getNowPlayerComponent().freeze(false);
                }
                getOff(m);
            });
        }
    }

}
