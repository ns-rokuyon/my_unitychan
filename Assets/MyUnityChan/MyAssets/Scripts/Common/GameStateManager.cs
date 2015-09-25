using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GameStateManager : SingletonObjectBase<GameStateManager> {
        public enum GameState {
            NO_SET,
            MAIN,
            MENU,
            MAP
        };

        public Player player { get; set; }
        private GameState state { get; set; }

        void Awake() {
            state = GameState.NO_SET;
        }

        void Start() {
            state = GameState.MAIN;
        }

        void Update() {
            watchStateTransition();
        }

        public static GameState now() {
            return self().state;
        }

        public static void change(GameState st) {
            self().state = st;
        }

        private void watchStateTransition() {
            Controller controller = player.getController();
            switch ( state ) { 
                case GameState.MAIN: 
                    if ( controller.keyPause() ) {
                        state = GameState.MENU;
                    }
                    break;
                case GameState.MENU:
                    if ( controller.keyPause() ) {
                        state = GameState.MAIN;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}