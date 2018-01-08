using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyJump : EnemyActionBase {
        public bool jumping { get; private set; }

        private float jumpFx;
        private float jumpFy;
        private Const.ID.Effect effect_name;

        public override string name() {
            return "JUMP";
        }

        public EnemyJump(Character character, float fx, float fy, Const.ID.Effect effect = Const.ID.Effect._NO_EFFECT)
            : base(character) {
            jumpFx = fx;
            jumpFy = fy;
            effect_name = effect;
        }

        public override void performFixed() {
            enemy.rigid_body.AddForce(new Vector3(enemy.transform.forward.x * jumpFx, jumpFy, 0), ForceMode.Impulse);
        }

        public override void perform() {
            if ( !jumping ) {
                jumping = true;
                if ( enemy is IEnemyJump )
                    (enemy as IEnemyJump).onJump();
            }
        }

        public override void off_perform() {
            jumping = false;
        }

        public override bool condition() {
            return controller.keyJump() && enemy.isGrounded() && !enemy.isHitstopping() && !enemy.isStunned();
        }

        public override void effect() {
            EffectManager.createEffect(
                effect_name,
                enemy.transform.position, 60, true);
        }
    }
}
