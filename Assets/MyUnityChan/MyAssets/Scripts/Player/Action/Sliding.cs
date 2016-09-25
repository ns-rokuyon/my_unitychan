using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public class PlayerSliding : PlayerAction {

        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                stun = 100;
                frame = 5;
                effect_name = Const.ID.Effect.HIT_01;
            }

            public override void attack(Character character, Hitbox hitbox) {
                if ( character ) {
                    character.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 5.0f, 0.0f), ForceMode.Impulse);
                    character.stun(stun);
                    character.damage(damage);
                }
                (EffectManager.Instance as EffectManager).createEffect(
                    effect_name,
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        private AttackSpec spec = null;
        private string hitbox_resource_path;
        private WaitForSeconds wait_hitbox;

        public PlayerSliding(Character character)
            : base(character) {
            priority = 5;
            skip_lower_priority = true;
            hitbox_resource_path = Const.Prefab.Hitbox["KICK"];
            spec = new Spec();
            wait_hitbox = new WaitForSeconds(0.1f);
        }

        public override string name() {
            return "SLIDING";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SLIDING;
        }

        public override void performFixed() {
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * 10.0f, ForceMode.VelocityChange);
        }

        public override void perform() {
            InvokerManager.createCoroutine(createHitbox());
            player.voice(Const.ID.PlayerVoice.ATTACK3);
            player.getAnimator().CrossFade("Sliding", 0.001f);
            player.lockInput(40);
        }

        public IEnumerator createHitbox() {
            yield return wait_hitbox;
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.7f * fw.x, 0.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }

        public override bool condition() {
            return controller.keyDown() && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }

    }
}