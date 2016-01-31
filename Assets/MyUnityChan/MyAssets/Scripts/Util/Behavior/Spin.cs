using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Spin : ObjectBase {
        public Vector3 angle;
        public bool noise;
        public float noise_weight;

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

            if ( noise )
                transform.Rotate(angle.changeZ(angle.z + Mathf.PerlinNoise(Time.time, 0.0f) * noise_weight) * Time.deltaTime);
            else
                transform.Rotate(angle * Time.deltaTime);
        }
    }
}
