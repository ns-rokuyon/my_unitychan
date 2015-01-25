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
        }

        public override bool condition() {
            return player.getHP() == 0;
        }

    }
}
