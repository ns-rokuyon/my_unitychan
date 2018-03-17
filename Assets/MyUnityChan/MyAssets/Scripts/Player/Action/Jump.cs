using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class PlayerJump : PlayerAction {
        protected float jump_start_y;
        protected Vector3 effect_offset = new Vector3(0.0f, 0.2f, 0.0f);
        protected Const.ID.Effect effect_name = Const.ID.Effect.JUMP_SMOKE_PUFF;

        private readonly Dictionary<Const.CharacterName, Vector3> jumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 600.0f, 0) },       // Mass: 46kg
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 130.0f, 0) }   // Mass: 10kg
        };

        private readonly Dictionary<Const.CharacterName, Vector3> dashJumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(100.0f, 400.0f, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(30.0f, 120.0f, 0) }
        };

        protected PlayerGrapple grappling;

        public PlayerJump(Character character)
            : base(character) {
        }

        public override string name() {
            return "JUMP";
        }

        public override void init() {
            grappling = player.action_manager.getAction<PlayerGrapple>("GRAPPLE");
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.JUMP;
        }

        public override void performFixed() {
            jump_start_y = player.transform.position.y;
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.rigid_body.AddForce(dashJumpF[player.character_name].mul(player.transform.forward.x, 1, 1), ForceMode.Impulse);
            }
            else {
                // jump (ground jump or air jump)
                player.rigid_body.AddForce(jumpF[player.character_name], ForceMode.Impulse);
            }
        }

        public override void perform() {
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.getAnimator().Play("DashJump", -1, 0.0f);
            }
            else {
                // jump (ground jump or air jump)
                player.getAnimator().Play("Jump", -1, 0.0f);
            }
            player.setAnimSpeedDefault();
            player.lockInput(2);
        }

        public override bool condition() {
            if ( grappling != null && grappling.grappled && controller.keyJump() ) {
                return true;
            }
            return controller.keyJump() && player.isGrounded() && !player.isHitRoof() && !player.isGuarding();
        }

        public override void effect() {
            EffectManager.createEffect(
                effect_name,
                player.transform.position + effect_offset, 60, true);
        }
    }


    public class PlayerDoubleJump : PlayerJump {
        private bool air_jumped;

        private readonly Dictionary<Const.CharacterName, Vector3> jumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 600.0f, 0) },       // Mass: 46kg
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 130.0f, 0) }   // Mass: 10kg
        };

        private readonly Dictionary<Const.CharacterName, Vector3> secondjumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 600.0f, 0) },       // Mass: 46kg
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 130.0f, 0) }   // Mass: 10kg
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
                player.rigid_body.AddForce(dashJumpF[player.character_name].mul(player.transform.forward.x, 1, 1), ForceMode.Impulse);
            }
            else {
                // jump (ground jump or air jump)
                if ( player.isGrounded() ) {
                    player.rigid_body.AddForce(jumpF[player.character_name], ForceMode.Impulse);
                }
                else {
                    player.rigid_body.velocity = Vector3.zero;
                    player.rigid_body.angularVelocity = Vector3.zero;
                    player.rigid_body.AddForce(secondjumpF[player.character_name], ForceMode.Impulse);
                    player.voice(Const.ID.PlayerVoice.JUMP);
                }
            }
        }

        public override void perform() {
            if ( player.isDash() ) {
                // dashdump (ground jump)
                player.getAnimator().Play("DashJump", -1, 0.0f);
            }
            else {
                // jump (ground jump or air jump)
                player.getAnimator().Play("Jump", -1, 0.0f);
            }
            player.setAnimSpeedDefault();
            player.lockInput(2);
        }

        public override bool condition() {
            return controller.keyJump() &&
                readyToJump() && !air_jumped &&
                !player.isHitRoof() && !player.isGuarding();
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

            float scvy = Mathf.Abs(player.rigid_body.velocity.y);
            if ( scvy < 16.0f ) {
                return true;
            }

            return false;
        }
    }

    public class PlayerWallJump : PlayerAction {
        protected Vector3 effect_offset = new Vector3(0.0f, 0.2f, 0.0f);
        protected Const.ID.Effect effect_name = Const.ID.Effect.JUMP_SMOKE_PUFF;

        private readonly Dictionary<Const.CharacterName, Vector3> jumpF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(0, 600.0f, 0) },       // Mass: 46kg
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(0, 130.0f, 0) }   // Mass: 10kg
        };

        public Vector3 F {
            get {
                return player.getBackVector() * 300.0f + jumpF[player.character_name];
            }
        }

        public PlayerWallJump(Character character)
            : base(character) {
        }

        public override string name() {
            return "WALL_JUMP";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.WALL_JUMP;
        }

        public override bool condition() {
            Action jump = player.action_manager.getAction("JUMP");
            if ( jump != null && jump.activation ) {
                return controller.keyJump() && !jump.condition() && player.isTouchedWall();
            }
            return false;
        }

        public override void performFixed() {
            // Wall jump
            player.rigid_body.velocity = Vector3.zero;
            player.rigid_body.AddForce(F, ForceMode.Impulse);
            player.flipDirection();
        }

        public override void perform() {
            // Wall jump
            player.getAnimator().Play("Jump", -1, 0.0f);
            player.setAnimSpeedDefault();
            player.voice(Const.ID.PlayerVoice.JUMP);
            player.lockInput(20);
        }

        public override void effect() {
            EffectManager.createEffect(
                effect_name,
                player.transform.position + effect_offset, 60, true);
        }
    }
}
