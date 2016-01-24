using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Spin : ObjectBase {
        public Vector3 angle;
        private RealDeltaTime pause_canceller;

        void Awake() {
            pause_canceller = GetComponent<RealDeltaTime>();
        }

        // Update is called once per frame
        void Update() {
            if ( PauseManager.isPausing() && pause_canceller ) {
                transform.Rotate(angle * pause_canceller.delta_time);
                return;
            }

            transform.Rotate(angle * Time.deltaTime);
        }
    }
}
