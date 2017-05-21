using UnityEngine;
using System.Collections;

using UniRx;
using System;

namespace MyUnityChan {
    public class PlayerDead : PlayerAction {
        public PlayerDead(Character character) : base(character) {
        }

        public override bool condition() {
            return player.getAllHP() == 0 && !player.manager.gameover;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.DEAD;
        }

        public override void perform() {
            player.delay(10, () => player.getAnimator().Play("Dead"));
            player.delay(30, () => player.voice(Const.ID.PlayerVoice.DAMAGED5));
            player.lockInput(150);
            player.manager.gameover = true;
            player.freeze();

            var cam = player.getPlayerCamera();
            player.delay(10, () => {
                cam.zoom(cam.getZoomPointFocusTo(player.transform, 0.2f), 5.0f);
                cam.fadeOut(180, delay_frame: 60);
            });
        }

        public override string name() {
            return "DEAD";
        }
    }
}