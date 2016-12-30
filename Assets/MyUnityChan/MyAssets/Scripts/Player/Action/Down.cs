using UnityEngine;
using System.Collections;
using System;
using UniRx;

namespace MyUnityChan {
    public class PlayerDown : PlayerAction {

        public PlayerDown(Character character)
            : base(character) {
        }

        public override string name() {
            return "DOWN";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.DOWN;
        }

        public override void perform() {
            player.getAnimator().CrossFade("Down", 0.001f);
            player.lockInput(150);
            if ( !player.isFrozen() ) {
                Observable.TimerFrame(64)
                    .Subscribe(_ => player.respawn());
            }
            player.freeze();
        }

        public override bool condition() {
            return player.getAllHP() == 0;
        }

    }
}
