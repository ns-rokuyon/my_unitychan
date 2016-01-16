using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerHadouken : PlayerAction {
        public static readonly string hitbox_resource_path = Const.Prefab.Hitbox["PROJECTILE"];
        public AttackSpec spec = null;

        public PlayerHadouken(Character character)
            : base(character) {
            spec = new Spec();
            priority = 10;
            skip_lower_priority = true;
        }

        public override string name() {
            return "HADOUKEN";
        }

        public class Spec : AttackSpec {
            public Spec() {
                damage = 50;
                stun = 50;
                frame = 9999;
            }

            public override void attack(Character character, Hitbox hitbox) {
                ((Enemy)character).stun(stun);
                ((Enemy)character).damage(damage);
                EffectManager.self().createEffect(Const.Prefab.Effect["HIT_02"],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        public override void perform() {
            Vector3 fw = player.transform.forward;
            player.getAnimator().Play("Hadouken");
            player.lockInput(30);
            InvokerManager.createFrameDelayVector3Invoker(15, fw, shootProjectile);
        }

        public override void performFixed() {
            Vector3 fw = player.transform.forward;
            player.GetComponent<Rigidbody>().AddForce(fw * -50.0f);
        }

        public override bool condition() {
            AnimatorStateInfo anim_state = player.getAnimator().GetCurrentAnimatorStateInfo(0);

            List<Controller.InputCode> cmd;
            if ( player.isLookAhead() ) {
                cmd = new List<Controller.InputCode> {
                    Controller.InputCode.DOWN,
                    Controller.InputCode.RIGHT,
                    Controller.InputCode.ATTACK
                };
            }
            else {
                cmd = new List<Controller.InputCode> {
                    Controller.InputCode.DOWN,
                    Controller.InputCode.LEFT,
                    Controller.InputCode.ATTACK
                };
            }

            //controller.keyProjectile() &&
            bool cond =
                command_recorder != null &&
                command_recorder.command(cmd) &&
                !player.getAnimator().GetBool("Turn") &&
                player.isGrounded() &&
                (player.isLookAhead() || player.isLookBack()) &&
                anim_state.nameHash != Animator.StringToHash("Base Layer.Hadouken");

            return cond;
        }

        void shootProjectile(Vector3 direction) {
            GameObject projectile = ObjectPoolManager.getGameObject(Hadouken.resource_path);
            projectile.setParent(Hierarchy.Layout.PROJECTILE);

            Hadouken prjc = projectile.GetComponent<Hadouken>();

            prjc.setDir(direction);
            prjc.setStartPosition(player.transform.position);
            prjc.setPlayerInfo(player);

            // hitbox
            HitboxManager.self().create<ProjectileHitbox>(hitbox_resource_path, use_objectpool:true).ready(projectile, spec);
        }
    }
}