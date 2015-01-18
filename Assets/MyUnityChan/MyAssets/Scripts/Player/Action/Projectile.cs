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
            player.rigidbody.AddForce(fw * -50.0f);
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
            GameObject projectile = UnityEngine.Object.Instantiate(player.projectile_prefab) as GameObject;
            GameObject projectile_particle = UnityEngine.Object.Instantiate(player.projectile_particle_prefab) as GameObject;

            Projectile prjc = projectile.GetComponent<Projectile>();
            Projectile particle = projectile_particle.GetComponent<Projectile>();

            prjc.init(player.transform.position, direction);

            // hitbox
            createHitbox(projectile);

            // particles
            particle.init(player.transform.position, direction, 0.001f);
            if ( player.transform.forward.x < 0.0f ) {
                particle.transform.Rotate(0.0f, 90.0f, 0.0f);
            }
            else {
                particle.transform.Rotate(0.0f, -90.0f, 0.0f);
            }
        }

        private void createHitbox(GameObject proj) {
            GameObject hitbox = GameObject.Instantiate(player.projectile_hitbox_prefab) as GameObject;
            ProjectileHitbox hitbox_script = hitbox.GetComponent<ProjectileHitbox>();
            hitbox_script.create(proj, spec);
        }
    }
}