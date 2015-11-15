using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerAttack : PlayerAction {
        private PlayerPunchL left_punch;
        private PlayerPunchR right_punch;
        private PlayerSpinKick spinkick;

        public override string name() {
            return "ATTACK";
        }

        public PlayerAttack(Character character)
            : base(character) {
            left_punch = new PlayerPunchL(character);
            right_punch = new PlayerPunchR(character);
            spinkick = new PlayerSpinKick(character);
        }

        public override void performFixed() {
            if ( player.isAnimState("Base Layer.PunchR") ) {
                spinkick.performFixed();
            }
            else if ( player.isAnimState("Base Layer.PunchL") ) {
                right_punch.performFixed();
            }
            else {
                left_punch.performFixed();
            }
        }

        public override void perform() {
            if ( player.isAnimState("Base Layer.PunchR") ) {
                spinkick.perform();
            }
            else if ( player.isAnimState("Base Layer.PunchL") ) {
                right_punch.perform();
            }
            else {
                left_punch.perform();
            }
        }

        public override bool condition() {
            bool cond = false;
            if ( player.isAnimState("Base Layer.PunchR") ) {
                cond = spinkick.condition();
            }
            else if ( player.isAnimState("Base Layer.PunchL") ) {
                cond = right_punch.condition();
            }
            else if ( player.isAnimState("Base Layer.SpinKick") ) {
                cond = false;
            }
            else {
                cond = left_punch.condition();
            }
            return cond;
        }

    }


    public class PlayerPunchL : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                stun = 60;
                frame = 5;
                effect_name = "HIT_01";
            }

            public override void attack(Character character, Hitbox hitbox) {
                character.GetComponent<Rigidbody>().AddForce(new Vector3(hitbox.forward.x * 5.0f, 5.0f, 0.0f), ForceMode.Impulse);
                character.stun(stun);
                character.damage(damage);
                (EffectManager.Instance as EffectManager).createEffect(Const.Prefab.Effect[effect_name],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        public AttackSpec spec = null;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Punch_Hitbox";

        public PlayerPunchL(Character character)
            : base(character) {
            spec = new Spec();
        }

        public override string name() {
            return "PUNCH_L";
        }

        public override void perform() {
            player.getAnimator().Play("PunchL");
            InvokerManager.createFrameDelayInvoker(3, createHitbox);
        }

        public override bool condition() {
            bool cond =
                controller.keyAttack() &&
                !player.getAnimator().GetBool("Turn");
            return cond;
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready( player.transform.position, fw, new Vector3(0.4f * fw.x, 1.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }

    public class PlayerPunchR : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 30;
                stun = 120;
                frame = 5;
                effect_name = "HIT_01";
            }

            public override void attack(Character character, Hitbox hitbox) {
                character.GetComponent<Rigidbody>().AddForce(new Vector3(hitbox.forward.x * 2.0f, 7.0f, 0.0f), ForceMode.Impulse);
                character.stun(stun);
                character.damage(damage);
                (EffectManager.Instance as EffectManager).createEffect(Const.Prefab.Effect[effect_name],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        public AttackSpec spec = null;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Punch_Hitbox";

        public PlayerPunchR(Character character)
            : base(character) {
            spec = new Spec();
        }

        public override string name() {
            return "PUNCH_R";
        }

        public override void perform() {
            player.getAnimator().Play("PunchR");
            InvokerManager.createFrameDelayInvoker(6, createHitbox);
            //player.getMoveController().register(new Player.DelayNormalEvent(6, createHitbox));
        }

        public override bool condition() {
            bool cond =
                controller.keyAttack() &&
                !player.getAnimator().GetBool("Turn");
            return cond;
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.6f * fw.x, 1.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }

    public class PlayerSpinKick : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 70;
                stun = 120;
                frame = 12;
                effect_name = "HIT_01";
            }

            public override void attack(Character character, Hitbox hitbox) {
                character.GetComponent<Rigidbody>().AddForce(new Vector3(hitbox.forward.x * 2.0f, 7.0f, 0.0f), ForceMode.Impulse);
                character.stun(stun);
                character.damage(damage);
                (EffectManager.Instance as EffectManager).createEffect(Const.Prefab.Effect[effect_name],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }

        public AttackSpec spec = null;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Kick_Hitbox";

        public PlayerSpinKick(Character character)
            : base(character) {
            spec = new Spec();
        }

        public override string name() {
            return "SPIN_KICK";
        }

        public override void perform() {
            player.getAnimator().Play("SpinKick");
            InvokerManager.createFrameDelayInvoker(20, createHitbox);
        }

        public override bool condition() {
            bool cond =
                controller.keyAttack() &&
                !player.getAnimator().GetBool("Turn");
            return cond;
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.6f * fw.x, 0.8f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }
}