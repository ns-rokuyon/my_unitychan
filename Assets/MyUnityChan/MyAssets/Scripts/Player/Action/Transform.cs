using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerTransform : PlayerAction {
        public PlayerTransform(Character ch) : base(ch) {
        }

        public override void perform() {
            player.manager.switchPlayerCharacter();
        }

        public override bool condition() {
            return controller.keyTransform() && player.isGrounded() &&
                !player.isGuarding() && !player.isFrozen() && !player.isHitstopping();
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.TRANSFORM;
        }

        public override string name() {
            return "TRANSFORM";
        }
    }
}