using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyJump : EnemyActionBase {
        private float jumpFx;
        private float jumpFy;

        public override string name() {
            return "JUMP";
        }

        public EnemyJump(Character character, float fx, float fy)
            : base(character) {
                jumpFx = fx;
                jumpFy = fy;
        }

        public override void performFixed() {
            enemy.GetComponent<Rigidbody>().AddForce(new Vector3(enemy.transform.forward.x * jumpFx, jumpFy, 0), ForceMode.Impulse);
        }

        public override bool condition() {
            return controller.keyJump() && enemy.isGrounded();
        }
    }
}
