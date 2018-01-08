﻿using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using System;

namespace MyUnityChan {
    public class PlayerBrake : PlayerAction {
        private const float BRAKE_POWER_DEFAULT = 500.0f;
        private float brake_power;

        public PlayerBrake(Character character)
            : base(character) {
            brake_power = BRAKE_POWER_DEFAULT;
            flag = null;
        }

        public override string name() {
            return "BRAKE";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.BRAKE;
        }

        public override void performFixed() {
            Vector3 fw = player.transform.forward;
            float vx = player.rigid_body.velocity.x;

            if ( Mathf.Abs(vx) < 0.001f ) {
                player.rigid_body.angularVelocity = Vector3.zero;
                player.rigid_body.velocity = Vector3.zero;
                return;
            }

            // brake down if no input
            if ( Mathf.Sign(fw.x) == Mathf.Sign(vx) ) {
                player.rigid_body.AddForce(fw * -1 * brake_power);
            }
            else {
                player.rigid_body.AddForce(fw * brake_power);
            }
        }

        public override bool condition() {
            float horizontal = controller.keyHorizontal();
            float vx = player.rigid_body.velocity.x;
            return player.isGrounded() &&
                !player.isInputLocked() &&
                (Mathf.Abs(horizontal) < 0.2 && Mathf.Abs(vx) > 0.2f);
        }
    }

    public class PlayerAccel : PlayerAction {
        private float maxspeed = 5.0f;
        private float terminal_velocity = 10.0f;
        private Vector3 moveF;
        private Rigidbody rb;

        private readonly Dictionary<Const.CharacterName, Vector3> __moveF = new Dictionary<Const.CharacterName, Vector3>{
            { Const.CharacterName.UNITYCHAN, new Vector3(4000f, 0, 0) },
            { Const.CharacterName.MINI_UNITYCHAN, new Vector3(400f, 0, 0) }
        };

        public PlayerAccel(Character character)
            : base(character) {
            moveF = __moveF[character.character_name];
            rb = player.rigid_body;
        }

        public override string name() {
            return "ACCEL";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.ACCEL;
        }

        public override void performFixed() {
            float horizontal = controller.keyHorizontal();
            Vector3 fw = player.transform.forward;
            float vx = rb.velocity.x;

            //if ( Mathf.Abs(horizontal) >= 0.2 && horizontal * vx < maxspeed ) {
            if ( Mathf.Abs(horizontal) >= 0.2 ) {
                if ( player.isGrounded() && Mathf.Sign(horizontal) != Mathf.Sign(vx) && Mathf.Abs(vx) > 0.1f ) {
                    // when player is turning, add low force
                    if ( player.isDash() ) {
                        rb.AddForce(horizontal * moveF / 8.0f);
                    }
                    else {
                        rb.AddForce(horizontal * moveF / 4.0f);
                    }
                }
                else {
                    // accelerate
                    if ( !player.isTouchedWall() ) {
                       rb.AddForce(horizontal * moveF);
                    }
                }
            }

            if ( !player.isGrounded() ) {
                if ( rb.velocity.y > -terminal_velocity) {
                    float coef = 23.9f;
                    // Quick falling down
                    rb.AddForce(Vector3.down * coef * rb.mass, ForceMode.Force);
                }
            }
        }

        public override void perform() {
            float horizontal = controller.keyHorizontal();
            float vx = rb.velocity.x;

            player.getAnimator().SetFloat("Speed", Mathf.Abs(horizontal));

            //if ( Mathf.Abs(horizontal) >= 0.2 && horizontal * vx < maxspeed ) {
            if ( Mathf.Abs(horizontal) >= 0.2 ) {
                if ( Mathf.Sign(horizontal) != Mathf.Sign(vx) && Mathf.Abs(vx) > 0.1f ) {
                }
                else {
                    player.getAnimator().SetBool("Turn", false);
                    player.setTurnDirSwitched(false);
                }

                if ( player.isGrounded() ) {
                    (player as ICharacterFootstep).onFootstep(Const.ID.FieldType.ASPHALT);
                    (player as ICharacterWalk).onForward();
                }

            }
            else {
                player.getAnimator().SetBool("Turn", false);
                player.setTurnDirSwitched(false);
            }
        }

        public override void off_perform() {
            (player as ICharacterWalk).onStay();
        }

        public override bool condition() {
            return !player.isInputLocked() && !player.isGuarding() && !player.isGrappling();
        }
    }

    public class PlayerDash : PlayerAction {
        private bool dash;
        private Vector3 moveF = new Vector3(200f, 100f, 0);

        public PlayerDash(Character character)
            : base(character) {
            dash = false;
            Observable.Interval(System.TimeSpan.FromSeconds(0.5))
                .Where(_ => condition())
                .Where(_ => player.getVx(abs:true) > 0.1f)
                .Subscribe(_ => {
                EffectManager.createEffect(
                    Const.ID.Effect.DASH_SMOKE_PUFF,
                    player.transform.position.add(0.0f, 0.2f, 0.0f),
                    60, true);
                }
            );
        }

        public override string name() {
            return "DASH";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.DASH;
        }

        public override void perform() {
            dash = true;
            player.getAnimator().speed = player.getAnimSpeedDefault() * 1.2f;
        }

        public override void performFixed() {
            float horizontal = controller.keyHorizontal();
            if ( !player.isTouchedWall() ) {
                player.rigid_body.AddForce(horizontal * moveF);
            }
        }

        public override void off_perform() {
            player.getAnimator().speed = player.getAnimSpeedDefault();
            if ( dash ) {
                Observable.TimerFrame(3)
                    .Subscribe(_ => offDash());
                if ( player.getAnimator().GetBool("Jump") ) {
                    return;
                }
                //player.getAnimator().CrossFade("Locomotion", 0.01f);
            }
        }

        public override bool condition() {
            return controller.keyDash() && player.isGrounded() && !player.isGuarding();
        }

        public bool isDash() {
            return dash;
        }

        public void offDash() {
            dash = false;
        }
    }


    public class PlayerLimitSpeed : PlayerAction {
        public float maxspeed = 5.0f;
        public float dash_maxspeed = 10.0f;
        public float dashjump_maxspeed = 10.0f;

        public PlayerLimitSpeed(Character character)
            : base(character) {
            flag = null;
        }

        public override string name() {
            return "LIMIT_SPEED";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.LIMIT_SPEED;
        }

        public override void performFixed() {
            if ( player.isDash() ) {
                limitSpeed(dash_maxspeed);
                return;
            }
            if ( player.isAnimState("Base Layer.DashJump") ) {
                limitSpeed(dashjump_maxspeed);
                return;
            }

            limitSpeed(maxspeed);
        }

        private void limitSpeed(float speed) {
            float vx = player.rigid_body.velocity.x;
            float vy = player.rigid_body.velocity.y;
            if ( Mathf.Abs(vx) > speed ) {
                player.rigid_body.velocity = new Vector3(Mathf.Sign(vx) * speed, vy);
            }
        }

        public override bool condition() {
            return !player.isStunned() && !player.isHitstopping();
        }
    }

    public class PlayerFall : PlayerAction {
        public bool falling { get; private set; }
        public float falling_distance { get; private set; }

        public PlayerFall(Character character) : base(character) {
        }

        public override bool condition() {
            return !player.isGrounded() &&
                   !player.isGrappling() &&
                   !player.isFlinching() &&
                   !player.isLanding() &&
                   player.ground_distance > 3.0f &&
                   player.getVy() < -4.0f;
        }

        public override void perform() {
            if ( falling )
                return;

            player.getAnimator().Play("Fall");
            falling = true;
        }

        public override void off_perform() {
            if ( player.isGrounded() )
                falling = false;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.FALL;
        }

        public override string name() {
            return "FALL";
        }
    }

    public class PlayerLand : PlayerAction {
        public bool landing { get; private set; }
        private PlayerFall fall;

        public PlayerLand(Character character) : base(character) {
            fall = player.action_manager.getAction<PlayerFall>("FALL");
        }

        public override void perform() {
            if ( landing )
                return;

            if ( player.ground_distance < 2.0f ) {
                player.getAnimator().Play("Locomotion");
                landing = true;
            }
        }

        public override void off_perform() {
            landing = false;
        }

        public override bool condition() {
            return !player.isGrounded() && player.getVy() < 0.0f;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.LAND;
        }

        public override string name() {
            return "LAND";
        }
    }
}
