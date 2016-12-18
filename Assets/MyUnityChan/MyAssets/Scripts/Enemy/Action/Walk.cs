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
            setParam(f, maxsp);
        }

        public void setParam(Vector3 f, float maxsp) {
            moveF = f;
            maxspeed = maxsp;
        }

        public void setParam(ZakoGroundTypeBase.Param param) {
            moveF = new Vector3(param.walk_fx, 0, 0);
            maxspeed = param.max_speed;
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
            if ( enemy.isGrounded() && Mathf.Abs(vx) > maxspeed ) {
                enemy.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
            }

            if ( enemy is IEnemyWalk )
                (enemy as IEnemyWalk).onForward();
        }

        public override void off_perform() {
            if ( enemy is IEnemyWalk )
                (enemy as IEnemyWalk).onStay();
        }

        public override bool condition() {
            return !enemy.isStunned() && !enemy.isHitstopping() && controller.keyHorizontal() != 0.0f;
        }
    }
}