using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public class PlayerSliding : PlayerAction {

        public PlayerSliding(Character character)
            : base(character) {
            priority = 5;
            skip_lower_priority = true;
        }

        public override string name() {
            return "SLIDING";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SLIDING;
        }

        public override void performFixed() {
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * 10.0f, ForceMode.VelocityChange);
        }

        public override void perform() {
            player.getAnimator().CrossFade("Sliding", 0.001f);
            player.lockInput(40);
        }

        public override bool condition() {
            return controller.keyDown() && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }

    }
}