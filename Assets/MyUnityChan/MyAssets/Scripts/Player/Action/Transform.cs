using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerTransform : PlayerAction {
        public PlayerTransform(Character ch) : base(ch) {
        }

        public override void perform() {
            player.delay(5, () => { player.manager.switchPlayerCharacter(); });
            player.manager.lockInput(30);
            EffectManager.createEffect(Const.ID.Effect.SWITCHING_PLAYER, player.ground_checker.point(), 60, true);
        }

        public override void performFixed() {
            player.rigid_body.velocity = Vector3.zero;
            player.rigid_body.angularVelocity = Vector3.zero;
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