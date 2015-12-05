using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class StayAndJumpAI : AIController {
        private GameObject target;

        // Use this for initialization
        void Start() {
            target = Player.getPlayer();
        }

        // Update is called once per frame
        void Update() {
            float x = self.transform.position.x;
            float diff = Mathf.Abs(target.transform.position.x - x);
            if ( diff < 4.0f ) {
                // If this enemy close to player, change jump flag to true
                inputs[(int)InputCode.JUMP] = true;
            }
            else {
                inputs[(int)InputCode.JUMP] = false;
            }
        }
    }
}