﻿using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerMissile : PlayerAction {
        public MissilePod shooter {
            get {
                return player.missile_pod;
            }
        }

        // bone refs
        private string left_shoulder_path = 
            "unitychan_dynamic/Character1_Reference/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_LeftShoulder/";
        private Transform left_arm;
        private Transform left_fore_arm;
        private Transform left_hand;

        public PlayerMissile(Character character)
            : base(character) {
            left_arm = player.transform.Find(left_shoulder_path + "Character1_LeftArm");
            left_fore_arm = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm");
            left_hand = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm/Character1_LeftHand");
        }

        public override string name() {
            return "MISSILE";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.MISSILE;
        }

        public override bool condition() {
            return controller.keyWeapon();
        }

        public override void perform() {
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
