using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerActionManager : ActionManager {

        protected override void start() {
        }

        protected override void update() {
        }

    }

    public abstract class PlayerAction : Action {
        protected Player player;
        protected PlayerController controller;

        public PlayerAction(Character character) {
            player = (Player)character;
            controller = (PlayerController)player.getController();
        }

    }
}