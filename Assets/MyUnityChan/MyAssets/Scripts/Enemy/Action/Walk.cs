using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyWalk : EnemyActionBase {
        private float maxspeed;
        private Vector3 moveF;

        public override string name() {
            return "WALK";
        }

        public EnemyWalk(Character character, Vector3 f, float maxsp)
            : base(character) {
                moveF = f;
                maxspeed = maxsp;
        }

        public override void performFixed() {
            float horizontal = controller.keyHorizontal();
            Vector3 fw = enemy.transform.forward;

            // accelerate
            if ( !enemy.isTouchedWall() ) {
                enemy.GetComponent<Rigidbody>().AddForce(horizontal * moveF);
            }

            float vx = enemy.GetComponent<Rigidbody>().velocity.x;
            float vy = enemy.GetComponent<Rigidbody>().velocity.y;
            if ( Mathf.Abs(vx) > maxspeed ) {
                enemy.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
            }
        }

        public override bool condition() {
            return !enemy.isStunned();
        }
    }
}