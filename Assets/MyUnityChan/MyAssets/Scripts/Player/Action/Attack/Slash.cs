using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public abstract class PlayerSlashBase : PlayerWeaponAttackBase {
        public PlayerSlashBase(Character character)
            : base(character) {
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

    public class PlayerTwoHandedSlashL : PlayerSlashBase {
        public override int hitbox_delay_frame { get { return 10; } }
        public override string anim_state_name { get { return "TwoHandedSwordSlashL"; } }

        private PlayerPickup pickup;

        public PlayerTwoHandedSlashL(Character character)
            : base(character) {
            pickup = player.action_manager.getAction<PlayerPickup>("PICKUP");
        }

        public override string name() {
            return "TWO_HANDED_SLASH_L";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_1, false, 5);
            player.delay(5, () => {
                GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_02, player, 0.5f, 0.5f, 120, true);
                obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.HORIZONTAL, mirror: player.isLookBack());
            });

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
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

    public class PlayerTwoHandedSlashM : PlayerSlashBase {
        public override int hitbox_delay_frame { get { return 15; } }
        public override string anim_state_name { get { return "TwoHandedSwordSlashM"; } }
        public override int anim_delay_frame { get { return 5; } }

        private PlayerPickup pickup;

        public PlayerTwoHandedSlashM(Character character)
            : base(character) {
            pickup = player.action_manager.getAction<PlayerPickup>("PICKUP");
        }

        public override string name() {
            return "TWO_HANDED_SLASH_M";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_1, false, 15);
            player.delay(15, () => {
                GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_02, player, 0.5f, 0.5f, 120, true);
                obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.VERTICAL, mirror:player.isLookBack());
            });

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
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

    public class PlayerTwoHandedSlashH : PlayerSlashBase {
        public override int input_lock_frame { get { return 40; } }
        public override int hitbox_delay_frame { get { return 20; } }
        public override string anim_state_name { get { return "TwoHandedSwordSlashH"; } }
        public override int anim_delay_frame { get { return 10; }
        }

        private PlayerPickup pickup;

        public PlayerTwoHandedSlashH(Character character) : base(character) {
            pickup = player.action_manager.getAction<PlayerPickup>("PICKUP");
        }

        public override string name() {
            return "TWO_HANDED_SLASH_H";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWING_2, false, 20);
            player.delay(20, () => {
                GameObject obj = EffectManager.createEffect(Const.ID.Effect.LIGHT_SLASH_02, player, 0.5f, 0.5f, 120, true);
                obj.GetComponent<SlashEffect>().rotate(Const.ID.SlashType.VERTICAL_UP, mirror: player.isLookBack());
            });

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
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