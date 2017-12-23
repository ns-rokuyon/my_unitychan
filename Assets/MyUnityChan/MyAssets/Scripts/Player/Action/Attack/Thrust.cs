using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class PlayerThrustBase : PlayerWeaponAttackBase {
        public PlayerThrustBase(Character character)
            : base(character) {
        }
    }

    // Light attacks
    // =============================================================
    public class PlayerThrustL : PlayerThrustBase {
        public override int hitbox_delay_frame { get { return 6; } }
        public override string anim_state_name { get { return "SpearThrustL"; } }

        public PlayerThrustL(Character character)
            : base(character) {
        }

        public override string name() {
            return "THRUST_L";
        }

        public override void perform() {
            base.perform();
            player.se(Const.ID.SE.SWISH_1, false, 6);

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
        }
    }

    // Middle attacks
    // =============================================================
    public class PlayerThrustM : PlayerThrustBase {
        public override int hitbox_delay_frame { get { return 20; } }
        public override int anim_delay_frame { get { return 3; } }
        public override string anim_state_name {
            get { 
                if ( !player.isGrounded() )
                    return "SpearStrikeH";
                return "SpearThrustM";
            }
        }

        private PlayerAttack _attack_manager;
        public PlayerAttack attack_manager {
            get {
                return _attack_manager ?? (_attack_manager = player.action_manager.getAction<PlayerAttack>("ATTACK"));
            }
        }

        public PlayerThrustM(Character character)
            : base(character) {
        }

        public override string name() {
            return "THRUST_M";
        }

        public override void perform() {
            base.perform();

            if ( anim_state_name == "SpearStrikeH" ) {
                player.se(Const.ID.SE.SWING_2, false, 22);
                attack_manager.breakLevelAttackPhase(30);
            } else {
                player.se(Const.ID.SE.SWISH_1, false, 20);
            }

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
        }
    }

    // Heavy attacks
    // =============================================================
    public class PlayerThrustH : PlayerThrustBase {
        public override int hitbox_delay_frame { get { return 22; } }
        public override int anim_delay_frame { get { return 5; } }
        public override string anim_state_name { get { return "SpearThrustH"; }
        }

        public PlayerThrustH(Character character)
            : base(character) {
        }

        public override string name() {
            return "THRUST_H";
        }

        public override void perform() {
            base.perform();

            player.se(Const.ID.SE.SWISH_2, false, 22);

            if ( weapon.follow_hand_targets.Count > 0 ) {
                player.moveIKLeftHandTo(weapon.follow_hand_targets[0].value, total_frame);
            }
        }
    }
}
