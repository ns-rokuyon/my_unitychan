using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public abstract class PlayerSlashBase : PlayerAction {
        public AttackSpec spec { get; set; }

        public virtual int total_frame { get { return hitbox_delay_frame + hitbox_frame; } }
        public virtual int input_lock_frame { get { return 10; } }
        public virtual int hitbox_frame { get { return 30; } }
        public virtual int hitbox_delay_frame { get { return 0; } }
        public virtual bool cancel_prev_attack { get { return true; } }
        public virtual float cross_fade_sec { get { return 0.001f; } }

        public abstract string anim_state_name { get; }

        public PlayerSlashBase(Character character)
            : base(character) {
            spec = null;
            use_transaction = true;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.getAnimator().CrossFade(anim_state_name, cross_fade_sec);
            player.lockInput(input_lock_frame);
            player.weapon.onAttack(hitbox_delay_frame, hitbox_frame, cancel_prev_attack, (hitbox, frame) => {
                hitbox.spec = spec;
            });
        }
    }

    // Light attacks
    // =============================================================
    public class PlayerSlashL : PlayerSlashBase {
        public override int hitbox_delay_frame { get { return 4; } }
        public override string anim_state_name { get { return "SwordSlashL"; } }

        public PlayerSlashL(Character character)
            : base(character) {
        }

        public override string name() {
            return "SLASH_L";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_1, false, 2);
            GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_01, player, 0.5f, 0.5f, 120, true);
            obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.HORIZONTAL, mirror:player.isLookBack());
        }
    }

    // Middle attacks
    // =============================================================
    public class PlayerSlashM : PlayerSlashBase {
        public override int input_lock_frame { get { return 16; } }
        public override int hitbox_delay_frame { get { return 4; } }
        public override string anim_state_name { get { return "SwordSlashM"; } }

        public PlayerSlashM(Character character)
            : base(character) {
        }

        public override string name() {
            return "SLASH_M";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_1, false, 6);
            player.delay(10, () => {
                GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_01, player, 0.5f, 0.5f, 120, true);
                obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.HORIZONTAL_REV, mirror:player.isLookBack());
            });
        }
    }

    // Heavy attacks
    // =============================================================
    public class PlayerSlashH : PlayerSlashBase {
        public override int input_lock_frame { get { return 30; } }
        public override int hitbox_delay_frame { get { return 8; } }
        public override string anim_state_name { get { return "SwordSlashH"; } }

        public PlayerSlashH(Character character) : base(character) {
        }

        public override string name() {
            return "SLASH_H";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_2, false, 5);
            player.delay(5, () => {
                GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_01, player, 0.5f, 0.5f, 120, true);
                obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.VERTICAL, mirror: player.isLookBack());
            });
        }

        public override void performFixed() {
            player.delay(5,
                () => { player.rigid_body.AddForce(new Vector3(0, -100, 0), ForceMode.Impulse); },
                FrameCountType.FixedUpdate);
        }
    }

    // Up attacks
    // =============================================================
    public class PlayerSlashUp : PlayerSlashBase {
        public override int input_lock_frame { get { return 20; } }
        public override int hitbox_delay_frame { get { return 20; } }
        public override string anim_state_name { get { return "SwordSlashUp"; } }

        public PlayerSlashUp(Character character)
            : base(character) {
        }

        public override string name() {
            return "SLASH_UP";
        }

        public override void perform() {
            base.perform();
            player.voice(Const.ID.PlayerVoice.ATTACK6, true, 20);
            player.se(Const.ID.SE.SWING_3, false, 10);
        }

        public override void performFixed() {
            player.delay(20,
                () => { player.rigid_body.AddForce(new Vector3(0, 400, 0), ForceMode.Impulse); },
                FrameCountType.FixedUpdate);
        }
    }
}