using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerRollForward : PlayerAction {
        public float rolling_power { get; set; }
        public CapsuleCollider collider { get; set; }

        public PlayerRollForward(Character ch) : base(ch) {
            priority = 7;
            skip_lower_priority = true;
            use_transaction = true;
            keep_skipping_lower_priority_in_transaction = true;
            rolling_power = 500.0f;
            collider = player.GetComponent<CapsuleCollider>();
        }

        public override void perform() {
            beginTransaction(30);
            player.lockInput(34);

            player.is_rolling = true;
            player.getAnimator().Play("RollForward");
            player.cancelGuard();
            player.se(Const.ID.SE.SWISH_1);
            player.status.invincible.enable(5);

            collider.height = 0.8f;
            collider.center = Vector3.up * 0.4f;
            player.delay(30, () => {
                // Revert collider size
                collider.height = 1.6f;
                collider.center = Vector3.up * 0.8f;

                player.is_rolling = false;
            });
        }

        public override void performFixed() {
            player.rigid_body.AddForce(player.transform.forward * rolling_power, ForceMode.Impulse);
        }

        public override bool condition() {
            bool key_forward = false;
            if ( player.isLookAhead() ) {
                key_forward = controller.keyHorizontal() > 0.1f;
            } else {
                key_forward = controller.keyHorizontal() < -0.1f;
            }
            return !player.isFlinching() && isFreeTransaction() 
                && controller.keyGuard() && key_forward;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.ROLL_FORWARD;
        }

        public override string name() {
            return "ROLL_FORWARD";
        }
    }

    public class PlayerRollBackward : PlayerAction {
        public float rolling_power { get; set; }
        public CapsuleCollider collider { get; set; }

        public PlayerRollBackward(Character ch) : base(ch) {
            priority = 7;
            skip_lower_priority = true;
            use_transaction = true;
            keep_skipping_lower_priority_in_transaction = true;
            rolling_power = 500.0f;
            collider = player.GetComponent<CapsuleCollider>();
        }

        public override void perform() {
            beginTransaction(30);
            player.lockInput(34);

            player.is_rolling = true;
            player.getAnimator().Play("RollBackward");
            player.cancelGuard();
            player.se(Const.ID.SE.SWISH_1);
            player.status.invincible.enable(5);

            collider.height = 0.8f;
            collider.center = Vector3.up * 0.4f;
            player.delay(30, () => {
                // Revert collider size
                collider.height = 1.6f;
                collider.center = Vector3.up * 0.8f;

                player.is_rolling = false;
            });
        }

        public override void performFixed() {
            player.rigid_body.AddForce(player.transform.forward * -1.0f * rolling_power, ForceMode.Impulse);
        }

        public override bool condition() {
            bool key_backward = false;
            if ( player.isLookAhead() ) {
                key_backward = controller.keyHorizontal() < -0.1f;
            } else {
                key_backward = controller.keyHorizontal() > 0.1f;
            }
            return !player.isFlinching() //&& isFreeTransaction() 
                && controller.keyGuard() && key_backward;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.ROLL_BACKWARD;
        }

        public override string name() {
            return "ROLL_BACKWARD";
        }
    }
}