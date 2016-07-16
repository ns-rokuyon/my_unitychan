using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class PlayerJump : PlayerAction {
        protected float jump_start_y;
        protected Vector3 effect_offset = new Vector3(0.0f, 0.2f, 0.0f);
        protected Const.ID.Effect effect_name = Const.ID.Effect.JUMP_SMOKE_PUFF;

        private readonly Dictionary<Const.CharacterName, Vector3> jumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 250.0f, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 90.0f, 0) }
        };

        private readonly Dictionary<Const.CharacterName, Vector3> dashJumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(100.0f, 100.0f, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(30.0f, 30.0f, 0) }
        };

        public PlayerJump(Character character)
            : base(character) {
        }

        public override string name() {
            return "JUMP";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.JUMP;
        }

        public override void performFixed() {
            jump_start_y = player.transform.position.y;
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.GetComponent<Rigidbody>().AddForce(dashJumpF[player.character_name].mul(player.transform.forward.x, 1, 1), ForceMode.Impulse);
            }
            else {
                // jump (ground jump or air jump)
                player.GetComponent<Rigidbody>().AddForce(jumpF[player.character_name], ForceMode.Impulse);
            }
        }

        public override void perform() {
            player.getAnimator().SetBool("Jump", true);
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.getAnimator().Play("DashJump", -1, 0.0f);
            }
            else {
                // jump (ground jump or air jump)
                player.getAnimator().Play("Jump", -1, 0.0f);
            }
            player.setAnimSpeedDefault();
            player.getAnimator().SetBool("OnGround", false);
            player.lockInput(2);
        }

        public override bool condition() {
            return controller.keyJump() && player.isGrounded() && !player.isGuarding();
        }

        public override void effect() {
            EffectManager.self().createEffect(
                effect_name,
                player.transform.position + effect_offset, 60, true);
        }
    }


    public class PlayerDoubleJump : PlayerJump {
        private bool air_jumped;

        private readonly Dictionary<Const.CharacterName, Vector3> secondjumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 250.0f, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 90.0f, 0) }
        };

        private readonly Dictionary<Const.CharacterName, Vector3> dashJumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(100.0f, 100.0f, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(30.0f, 30.0f, 0) }
        };

        public PlayerDoubleJump(Character character)
            : base(character) {
                air_jumped = false;
        }

        public override string name() {
            return "AIR_JUMP";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.DOUBLE_JUMP;
        }

        public override void performFixed() {
            jump_start_y = player.transform.position.y;
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.GetComponent<Rigidbody>().AddForce(new Vector3(player.transform.forward.x * 100.0f, 100.0f, 0), ForceMode.Impulse);
            }
            else {
                // jump (ground jump or air jump)
                if ( player.isGrounded() ) {
                    player.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 250.0f, 0), ForceMode.Impulse);
                }
                else {
                    Rigidbody rigidbody = player.GetComponent<Rigidbody>();
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    rigidbody.AddForce(new Vector3(0f, 270.0f, 0), ForceMode.Impulse);
                }
            }
        }

        public override void perform() {
            player.getAnimator().SetBool("Jump", true);
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.getAnimator().Play("DashJump", -1, 0.0f);
            }
            else {
                // jump (ground jump or air jump)
                player.getAnimator().Play("Jump", -1, 0.0f);
            }
            player.setAnimSpeedDefault();
            player.getAnimator().SetBool("OnGround", false);
            player.lockInput(2);
        }

        public override bool condition() {
            return controller.keyJump() &&
                readyToJump() && !air_jumped && !player.isGuarding();
        }

        public override void end() {
            if ( !player.isGrounded() ) {
                air_jumped = true;
            }
        }

        public override void constant_perform() {
            if ( player.isGrounded() ) {
                air_jumped = false;
            }
        }

        private bool readyToJump() {
            if ( player.isGrounded() ) {
                return true;
            }

            float scvy = Mathf.Abs(player.GetComponent<Rigidbody>().velocity.y);
            if ( scvy < 16.0f ) {
                return true;
            }

            return false;
        }
    }
}
