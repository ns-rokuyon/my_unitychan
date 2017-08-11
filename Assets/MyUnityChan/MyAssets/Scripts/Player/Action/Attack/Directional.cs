using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    // Direction(Up) attacks
    // =============================================================
    public class PlayerShoryu : PlayerAction {
        public class Spec : AttackSpec {
            public Spec() {
                damage = 70;
                stun = 0;
                frame = 12;
                hitstop = 20;
                knockback = 30;
                launch_fy = 7.0f;
                hit_se = Const.ID.SE.HIT_3;
                effect_name = Const.ID.Effect.IMPACT_01;
            }
        }

        public AttackSpec spec = null;
        protected int total_frame = 60;
        protected int delay_fixedupdate = 10;
        private static readonly string hitbox_resource_path = "Prefabs/Hitbox/Kick_Hitbox";


        public PlayerShoryu(Character character)
            : base(character) {
            spec = new Spec();
            priority = 10;
            skip_lower_priority = true;
            keep_skipping_lower_priority_in_transaction = true;
            use_transaction = true;
        }

        public override string name() {
            return "SHORYUKEN";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.delay(5, () => player.getAnimator().Play("Shoryu"));
            player.voice(Const.ID.PlayerVoice.ATTACK5, true, 20);
            player.lockInput(45);
            player.delay(22, () => createHitbox());
        }

        public override void performFixed() {
            Observable.EveryFixedUpdate()
                .DelayFrame(delay_fixedupdate)
                .Take(1)
                .Subscribe(_ => player.rigid_body.AddForce(new Vector3(0, 450, 0), ForceMode.Impulse))
                .AddTo(player);
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }

        private void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.6f * fw.x, 0.8f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
            hitbox.position_updater = (Hitbox hb) => {
                hb.transform.position = player.bone_manager.position(Const.ID.UnityChanBone.RIGHT_HAND);
            };
        }
    }

    // Direction(Down) attacks
    // =============================================================
    public class PlayerSliding : PlayerAction {

        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                stun = 60;
                frame = 5;
                hitstop = 20;
                knockback = 20;
                launch_fy = 5.0f;
                hit_se = Const.ID.SE.HIT_1;
                effect_name = Const.ID.Effect.HIT_01;
            }
        }

        private AttackSpec spec = null;
        private string hitbox_resource_path;

        public PlayerSliding(Character character)
            : base(character) {
            priority = 5;
            skip_lower_priority = true;
            use_transaction = true;
            keep_skipping_lower_priority_in_transaction = true;
            hitbox_resource_path = Const.Prefab.Hitbox["KICK"];
            spec = new Spec();
        }

        public override string name() {
            return "SLIDING";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SLIDING;
        }

        public override void performFixed() {
            player.rigid_body.AddForce(player.transform.forward * 10.0f, ForceMode.VelocityChange);
        }

        public override void perform() {
            beginTransaction(40);

            player.voice(Const.ID.PlayerVoice.ATTACK3);
            player.getAnimator().CrossFade("Sliding", 0.001f);
            player.lockInput(40);
            player.delay(6, () => { createHitbox(); });
        }

        public void createHitbox() {
            Vector3 fw = player.transform.forward;
            MeleeAttackHitbox hitbox = HitboxManager.self().create<MeleeAttackHitbox>(hitbox_resource_path);
            hitbox.ready(player.transform.position, fw, new Vector3(0.7f * fw.x, 0.0f, 0.0f), spec);
            hitbox.setOwner(player.gameObject);
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && player.isGrounded() && isFreeTransaction();
        }
    }
}