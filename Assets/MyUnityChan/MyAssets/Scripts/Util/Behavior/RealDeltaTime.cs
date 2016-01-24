using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class RealDeltaTime : ObjectBase {
        public float delta_time { get; set; }
        private float last_delta_time = 0.0f;

        void Start() {
            last_delta_time = Time.realtimeSinceStartup;
        }

        void Update() {
            delta_time = Time.realtimeSinceStartup - last_delta_time;
            last_delta_time = Time.realtimeSinceStartup;
        }
    }
}
