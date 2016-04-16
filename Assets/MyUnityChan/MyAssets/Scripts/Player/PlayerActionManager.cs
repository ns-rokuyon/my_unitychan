using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class PlayerActionManager : ActionManager {

        protected override void start() {
        }

        protected override void update() {
        }

        public void enableAction(Const.PlayerAction action_id) {
            var action = actions.FirstOrDefault(pair => ((PlayerAction)pair.Value).id() == action_id);
            if ( action.Value != null ) {
                action.Value.enable();
            }
        }

        public void disableAction(Const.PlayerAction action_id) {
            var action = actions.FirstOrDefault(pair => ((PlayerAction)pair.Value).id() == action_id);
            if ( action.Value != null ) {
                action.Value.disable();
            }
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

        public abstract Const.PlayerAction id();
    }
}