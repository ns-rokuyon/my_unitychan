using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PhysicsObject : ObjectBase {
        public Const.ID.SE ground_hit_se = Const.ID.SE._NO;

        void Awake() {
            setupSoundPlayer();
        }

        public void OnCollisionEnter(Collision collision) {
            switch ( collision.collider.tag ) {
                case "Ground":
                    se(ground_hit_se); break;
                default:
                    break;
            }
        }
    }
}