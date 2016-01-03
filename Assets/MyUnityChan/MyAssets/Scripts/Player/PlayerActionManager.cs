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
        protected CommandRecorder command_recorder;

        public PlayerAction(Character character) {
            player = (Player)character;
            controller = (PlayerController)player.getController();
            command_recorder = controller.getCommandRecorder();
            priority = 0;
            skip_lower_priority = false;
        }

    }
}