using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SimpleAI : AIController {
        private GameObject target;
        private int target_update_time_span;

        // Use this for initialization
        void Start() {
            target_update_time_span = 10;
        }

        // Update is called once per frame
        void Update() {
            if ( Time.frameCount % target_update_time_span == 0 ) {
                target = Enemy.findNearestPlayer(self.transform.position);
            }

            if ( target == null ) {
                horizontal_input = 0.0f;
                return;
            }

            float target_x = target.transform.position.x;
            float self_x = self.transform.position.x;

            if ( target_x < self_x ) {
                horizontal_input = -1.0f;
            }
            else {
                horizontal_input = +1.0f;
            }

        }
    }
}
