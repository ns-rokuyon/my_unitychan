using UnityEngine;
using System.Collections;
using System;
using RootMotion.FinalIK;

namespace MyUnityChan {
    public class PlayerThrow : PlayerAction {
        public float throw_fx = 200.0f;

        public PlayerThrow(Character character) : base(character) {
            priority = 6;
            skip_lower_priority = true;
            keep_skipping_lower_priority_in_transaction = true;
        }

        public override void performFixed() {
        }

        public override void perform() {
            beginTransaction(30);

            throwout();
            player.getAnimator().CrossFade("SwordSlashL", 0.001f);
            player.lockInput(30);
        }

        public override bool condition() {
            return player.weapon != null &&
                controller.keyGuard() &&
                controller.keyAttack() &&
                !player.isFlinching();
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.THROW;
        }

        public override string name() {
            return "THROW";
        }

        public void throwout() {
            if ( player.weapon )
                player.weapon.onThrownOutBy(player, throw_fx);

            // Release current interaction
            player.unequip();
        }
    }
}