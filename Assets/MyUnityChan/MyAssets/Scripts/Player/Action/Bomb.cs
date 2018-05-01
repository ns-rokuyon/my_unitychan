using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace MyUnityChan {
    public class PlayerBomb : PlayerAction {
        public PlayerBomb(Character character)
            : base(character) {
        }

        public Bomber bomber {
            get { return player.bomber; }
        }

        public override string name() {
            return "BOMB";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.BOMB;
        }

        public override void perform() {
            bool accepted = bomber.communicate();
            if ( accepted ) {
                player.lockInput(10);
                return;
            }

            Vector3 pos = bomber.getInitPosition(player.transform);
            Bomb bomb = bomber.put(pos);
            if ( bomb ) {
                player.lockInput(10);
            }
        }

        public override bool condition() {
            return controller.keyAttack();
        }
    }
}
