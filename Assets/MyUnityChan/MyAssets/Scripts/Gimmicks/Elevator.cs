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
                transform.position = transform.position.add(0, -speed * Time.deltaTime, 0);
            }
            else if ( state == State.GOING_DOWN ) {
                transform.position = transform.position.add(0, speed * Time.deltaTime, 0);
            }
        }

        public override void getOn(ObjectBase ob) {
            Debug.Log("getOn");
            if ( state == State.WAITING_BOTTOM )
                state = State.GOING_UP;
            else if ( state == State.WAITING_TOP )
                state = State.GOING_DOWN;
        }

    }

}
