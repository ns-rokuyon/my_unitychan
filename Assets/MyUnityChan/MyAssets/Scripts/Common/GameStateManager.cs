using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GameStateManager : SingletonObjectBase<GameStateManager> {
        public enum GameState {
            NO_SET,
            MAIN,
            MENU,
            SETTINGS,
            MAP
        };

        public PlayerManager player_manager { get; set; }
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

        // Global player getter
        public static Player getPlayer() {
            return getPlayerObject().GetComponent<Player>();
        }

        public static GameObject getPlayerObject() {
            if ( !self().player_manager ) {
                return null;
            }
            return self().player_manager.getNowPlayer();
        }

        private void watchStateTransition() {
            Controller controller = getPlayer().getController();
            switch ( state ) { 
                case GameState.MAIN: 
                    if ( controller.keyPause() ) {
                        /* 
                         * State: MAIN -> MENU 
                         * Trigger: pause key was pressed
                         */
                        state = GameState.MENU;
                        PauseManager.Instance.pause(true);
                        MenuManager.Instance.enter();
                    }
                    break;
                case GameState.MENU:
                    if ( controller.keyPause() ) {
                        /*
                         * State: MENU -> MAIN
                         * Trigger: pause key was pressed
                         */ 
                        state = GameState.MAIN;
                        MenuManager.Instance.quit();
                        PauseManager.Instance.pause(false);
                    }
                    break;
                case GameState.MAP:
                    if ( controller.keyCancel() ) {
                        /*
                         * State: MAP -> MENU
                         * Trigger: cancel key was pressed
                         */ 
                        state = GameState.MENU;
                        MapViewer.Instance.quit();
                        MenuManager.Instance.enter();
                    }
                    break;
                case GameState.SETTINGS:
                    if ( controller.keyCancel() ) {
                        /*
                         * State: SETTINGS -> MENU
                         * Trigger: cancel key was pressed
                         */
                        state = GameState.MENU;
                        SettingManager.Instance.quit();
                        MenuManager.Instance.enter();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}