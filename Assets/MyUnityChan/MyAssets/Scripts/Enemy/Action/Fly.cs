using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyFly : EnemyActionBase {
        public float maxspeed { get; private set; }
        public Vector3 flapF { get; private set; }
        public Vector3 flyF { get; private set; }

        private Rigidbody rigidbody { get; set; }

        public override string name() {
            return "FLY";
        }

        public EnemyFly(Character character, Vector3 _flyF, Vector3 _flapF, float _maxspeed)
            : base(character) {
            flyF = _flyF;
            flapF = _flapF;
            maxspeed = _maxspeed;
            rigidbody = enemy.GetComponent<Rigidbody>();
        }

        public void setParam(ZakoAirTypeBase.Param param) {
            flyF = param.flyF;
            flapF = param.flapF;
            maxspeed = param.max_speed;
        }

        public override void performFixed() {
            bool flap = controller.keyJump();
            float horizontal = controller.keyHorizontal();
            Vector3 fw = enemy.transform.forward;

            // accelerate
            if ( !enemy.isTouchedWall() ) {
                rigidbody.AddForce(horizontal * flyF);
            }

            // Flap
            if ( flap ) {
                rigidbody.AddForce(flapF, ForceMode.Impulse);
            }

            if ( maxspeed > 0.0f ) {
                float vx = rigidbody.velocity.x;
                float vy = rigidbody.velocity.y;
                if ( Mathf.Abs(vx) > maxspeed ) {
                    rigidbody.velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
                }
            }
        }

        public override bool condition() {
            return !enemy.isStunned() && !enemy.isHitstopping();
        }
    }
}
