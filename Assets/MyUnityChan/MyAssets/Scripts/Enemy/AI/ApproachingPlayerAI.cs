using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ApproachingPlayerAI : AIController {
        private GameObject target;
        private int target_update_time_span;
        private string area_name;

        // Use this for initialization
        void Start() {
            target_update_time_span = 10;
            area_name = AreaManager.Instance.getAreaNameFromObject(self.gameObject);
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

            if ( target.GetComponent<Player>().getAreaName() != area_name ) {
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
