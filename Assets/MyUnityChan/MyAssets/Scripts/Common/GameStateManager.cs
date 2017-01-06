using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class GameStateManager : SingletonObjectBase<GameStateManager> {
        public enum GameState {
            NO_SET,
            MAIN,
            MENU
        };

        public PlayerManager player_manager { get; set; }
        private GameState state { get; set; }

        public Const.Language language {
            get {
                return SettingManager.isSetupDone() ? SettingManager.get<Const.Language>(Settings.Select.LANG) : Const.Language.JP;
            }
        }

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

        public static bool isLoadingInBackground() {
            return AssetBundleManager.isNowLoading();
        }

        public static void change(GameState st) {
            self().state = st;
        }

        public static void showCurrentSelected() {
            var current_ui = EventSystem.current.currentSelectedGameObject;
            if ( current_ui ) Debug.Log(current_ui.name);
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
                    else {
                        EventSystem.current.SetSelectedGameObject(null);
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
                default:
                    break;
            }
        }
    }
}