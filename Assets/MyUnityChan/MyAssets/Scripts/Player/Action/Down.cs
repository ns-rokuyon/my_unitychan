using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerDown : PlayerAction {

        public PlayerDown(Character character)
            : base(character) {
        }

        public override string name() {
            return "DOWN";
        }

        public override void perform() {
            player.getAnimator().CrossFade("Down", 0.001f);
            player.lockInput(150);
            if ( !player.isFrozen() ) {
                InvokerManager.createFrameDelayInvoker(64, player.respawn);
            }
            player.freeze();
        }

        public override bool condition() {
            return player.getAllHP() == 0;
        }

    }
}
