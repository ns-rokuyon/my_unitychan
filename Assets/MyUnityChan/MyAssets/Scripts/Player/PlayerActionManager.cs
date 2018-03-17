using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace MyUnityChan {
    public class PlayerActionManager : ActionManager {
        public Player player {
            get {
                return character as Player;
            }
        }

        protected override void start() {
            this.ObserveEveryValueChanged(_ => player.manager.gameover)
                .Where(gameover => gameover == true)
                .Subscribe(_ => disableAllActions());
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

        public override Character owner {
            get {
                return player;
            }
        }

        public PlayerAction(Character character) : base() {
            player = (Player)character;
            controller = (PlayerController)player.getController();
            command_recorder = controller.getCommandRecorder();
        }

        public abstract Const.PlayerAction id();
    }
}