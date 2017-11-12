using UnityEngine;
using System.Collections;
using UniRx;


namespace MyUnityChan {
    // Light attacks
    // =============================================================
    public class PlayerPunchL : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                stun = 0;
                hitstop = 5;
                frame = 5;
                knockback = 20;
                launch_fy = 5.0f;
                force_degree = 45;
                force_power = 200;
                hit_se = Const.ID.SE.HIT_1;
                effect_name = Const.ID.Effect.IMPACT_02;
            }
        }

        public AttackSpec spec = null;
        protected int total_frame = 20;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Punch_Hitbox";

        public PlayerPunchL(Character character)
            : base(character) {
            spec = new Spec();
            use_transaction = true;
        }

        public override string name() {
            return "PUNCH_L";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.getAnimator().Play("PunchL");
            player.lockInput(6);
            player.delay(3, () => {
                createHitbox();
                player.se(Const.ID.SE.SWISH_1);
            });
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready( player.transform.position, fw, new Vector3(0.4f * fw.x, 1.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }

    // Middle attacks
    // =============================================================
    public class PlayerPunchR : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 30;
                stun = 0;
                hitstop = 8;
                frame = 5;
                knockback = 22;
                launch_fy = 7.0f;
                force_degree = 45;
                force_power = 400;
                hit_se = Const.ID.SE.HIT_2;
                effect_name = Const.ID.Effect.IMPACT_02;
            }
        }

        public AttackSpec spec = null;
        protected int total_frame = 20;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Punch_Hitbox";

        public PlayerPunchR(Character character)
            : base(character) {
            spec = new Spec();
            use_transaction = true;
        }

        public override string name() {
            return "PUNCH_R";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.getAnimator().Play("PunchR");
            player.voice(Const.ID.PlayerVoice.ATTACK, true, 6);
            player.lockInput(10);
            player.delay(6, () => {
                createHitbox();
                player.se(Const.ID.SE.SWISH_1);
            });
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.6f * fw.x, 1.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }

    // Heavy attacks
    // =============================================================
    public class PlayerSpinKick : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 70;
                stun = 0;
                frame = 12;
                hitstop = 20;
                knockback = 30;
                launch_fy = 7.0f;
                force_degree = 30;
                force_power = 1000;
                hit_se = Const.ID.SE.HIT_3;
                effect_name = Const.ID.Effect.IMPACT_01;
            }
        }

        public AttackSpec spec = null;
        protected int total_frame = 60; 
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Kick_Hitbox";

        public PlayerSpinKick(Character character)
            : base(character) {
            spec = new Spec();
            use_transaction = true;
        }

        public override string name() {
            return "SPIN_KICK";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.getAnimator().Play("SpinKick");
            player.voice(Const.ID.PlayerVoice.ATTACK4, true, 20);
            player.lockInput(45);
            player.se(Const.ID.SE.SWING_3, false, 16);
            player.delay(22, () => createHitbox());
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.6f * fw.x, 0.8f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }
    }


}