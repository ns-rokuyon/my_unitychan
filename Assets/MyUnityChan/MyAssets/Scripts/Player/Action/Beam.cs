using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerBeam : PlayerAction {
        public PlayerBeam(Character character)
            : base(character) {
        }

        public override string name() {
            return "BEAM";
        }

        public override bool condition() {
            return controller.keyTest();
        }

        // play beam-shot motion
        public override void performLate() {
            string left_shoulder_path = 
                "Character1_Reference/Character1_Hips/Character1_Spine/Character1_Spine1/Character1_Spine2/Character1_LeftShoulder/";
            Transform bone;

            // hand up to front
            if ( player.isLookAhead() ) {
                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm");
                bone.rotation = Quaternion.AngleAxis(180.0f, new Vector3(0, 0, 1));

                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm");
                bone.localRotation = Quaternion.AngleAxis(-90.0f, new Vector3(1, 0, 0));

                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm/Character1_LeftHand");
                bone.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
            else {
                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm");
                bone.rotation = Quaternion.AngleAxis(0.0f, new Vector3(0, 0, 1));

                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm");
                bone.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0));

                bone = player.transform.Find(left_shoulder_path + "Character1_LeftArm/Character1_LeftForeArm/Character1_LeftHand");
                bone.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
        }

    }
}