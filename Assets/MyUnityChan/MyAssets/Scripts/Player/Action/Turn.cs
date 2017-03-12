using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerTurn : PlayerAction {
        public PlayerTurn(Character character)
            : base(character) {
        }

        public override string name() {
            return "TURN";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.TURN;
        }

        public override void perform() {
            float vx = player.rigid_body.velocity.x;
            float vy = player.rigid_body.velocity.y;
            Vector3 fw = player.transform.forward;
            float input_horizontal = controller.keyHorizontal();

            if ( input_horizontal > 0 && fw.x < 0 ) {
                // input right when player turns left
                player.transform.rotation = Quaternion.LookRotation(new Vector3(input_horizontal, 0, 0.8f));
            }
            else if ( input_horizontal < 0 && fw.x > 0 ) {
                // input left when player turns right
                player.transform.rotation = Quaternion.LookRotation(new Vector3(input_horizontal, 0, -0.8f));
            }
            else {
                float newz_fw = fw.z;
                if ( newz_fw > 0 ) {
                    newz_fw -= 0.3f;
                    if ( newz_fw < 0 ) {
                        newz_fw = 0;
                    }
                }
                else {
                    newz_fw += 0.3f;
                    if ( newz_fw > 0 ) {
                        newz_fw = 0;
                    }
                }

                if ( Mathf.Abs(input_horizontal) >= 0.2f && input_horizontal * vx < 20f && Mathf.Sign(input_horizontal) != Mathf.Sign(vx) && Mathf.Abs(vx) > 0.1f ) {
                    if ( player.isGrounded() && player.isTurnDirSwitched() == false ) {
                        player.getAnimator().CrossFade("PlantNTurnRight180", 0.01f);
                        player.getAnimator().SetBool("Turn", true);
                    }
                }
                player.transform.rotation = Quaternion.LookRotation(new Vector3(fw.x, 0, newz_fw));
            }
        }

        public override bool condition() {
            return !player.isGuarding() && !player.isGrappling() && !player.isHitstopping();
        }
    }
}
