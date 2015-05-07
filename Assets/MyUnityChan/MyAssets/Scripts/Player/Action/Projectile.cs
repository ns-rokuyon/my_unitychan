using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerHadouken : PlayerAction {
        public AttackSpec spec = null;

        public PlayerHadouken(Character character)
            : base(character) {
            spec = new Spec();
        }

        public override string name() {
            return "HADOUKEN";
        }

        public class Spec : AttackSpec {
            public Spec() {
                damage = 5;
                stun = 50;
                frame = 100;
            }

            public override void attack(Character character, Hitbox hitbox) {
                ((Enemy)character).stun(stun);
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

            bool cond =
                controller.keyProjectile() &&
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

            // hitbox
            createHitbox(projectile);

        }

        private void createHitbox(GameObject proj) {
            GameObject hitbox = GameObject.Instantiate(player.projectile_hitbox_prefab) as GameObject;
            ProjectileHitbox hitbox_script = hitbox.GetComponent<ProjectileHitbox>();
            hitbox_script.create(proj, spec);
        }
    }
}