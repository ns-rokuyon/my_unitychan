using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerBeam : PlayerAction {
        private BeamTurret shooter;

        // bone refs
        private Transform left_arm;
        private Transform left_fore_arm;
        private Transform left_hand;

        public PlayerBeam(Character character)
            : base(character) {
            left_arm = player.bone_manager.bone(Const.ID.UnityChanBone.LEFT_ARM).transform;
            left_fore_arm = player.bone_manager.bone(Const.ID.UnityChanBone.LEFT_FORE_ARM).transform;
            left_hand = player.bone_manager.bone(Const.ID.UnityChanBone.LEFT_HAND).transform;

            shooter = character.GetComponent<BeamTurret>();
        }

        public override string name() {
            return "BEAM";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.BEAM;
        }

        public override bool condition() {
            return controller.keyProjectile();
        }

        public override void perform() {
            shooter.aimForward();
            shooter.trigger();
        }

        public override void off_perform() {
            shooter.trigger(false);
        }

        // play beam-shot motion
        public override void performLate() {
            // hand up to front
            if ( player.isLookAhead() ) {
                left_arm.rotation = Quaternion.AngleAxis(180.0f, new Vector3(0, 0, 1));
                left_fore_arm.localRotation = Quaternion.AngleAxis(-90.0f, new Vector3(1, 0, 0));
                left_hand.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
            else {
                left_arm.rotation = Quaternion.AngleAxis(0.0f, new Vector3(0, 0, 1));
                left_fore_arm.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0));
                left_hand.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
        }

    }
}