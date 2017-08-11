using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyFly : EnemyActionBase {
        public float maxspeed { get; private set; }
        public Vector3 flapF { get; private set; }
        public Vector3 flyF { get; private set; }

        public override string name() {
            return "FLY";
        }

        public EnemyFly(Character character, Vector3 _flyF, Vector3 _flapF, float _maxspeed)
            : base(character) {
            flyF = _flyF;
            flapF = _flapF;
            maxspeed = _maxspeed;
        }

        public void setParam(ZakoAirTypeBase.Param param) {
            flyF = param.flyF;
            flapF = param.flapF;
            maxspeed = param.max_speed;
        }

        public override void performFixed() {
            bool flap = controller.keyJump();
            float horizontal = controller.keyHorizontal();
            float vertical = controller.keyVertical();
            Vector3 fw = enemy.transform.forward;

            // accelerate
            enemy.rigid_body.AddForce(horizontal * flyF.onlyX() + vertical * flyF.onlyY());

            // Flap
            if ( flap ) {
                enemy.rigid_body.AddForce(flapF, ForceMode.Impulse);
            }

            if ( maxspeed > 0.0f ) {
                float vx = enemy.rigid_body.velocity.x;
                float vy = enemy.rigid_body.velocity.y;
                if ( Mathf.Abs(vx) > maxspeed ) {
                    enemy.rigid_body.velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
                }
            }
        }

        public override bool condition() {
            return !enemy.isStunned() && !enemy.isHitstopping();
        }
    }
}
