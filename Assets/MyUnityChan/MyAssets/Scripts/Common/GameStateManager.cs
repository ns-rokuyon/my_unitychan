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

        public PlayerDemo showcase;

        public PlayerManager player_manager { get; set; }
        private GameState state { get; set; }

        public Const.Language language {
            get {
                return SettingManager.isSetupDone() ? SettingManager.get<Const.Language>(Settings.Select.LANG) : Const.Language.JP;
            }
        }

        public static bool gameover {
            get { return Instance.player_manager != null ? Instance.player_manager.gameover : false; }
        }

        public static float fps {
            get { return 1.0f / Time.deltaTime; }
        }

        public static int approximatedFps {
            get { return (int)fps; }
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
            var obj = getPlayerObject();
            return obj == null ? null : obj.GetComponent<Player>();
        }

        public static GameObject getPlayerObject() {
            if ( !self().player_manager ) {
                return null;
            }
            return self().player_manager.getNowPlayer();
        }

        public static void hideHUD() {
            GUIObjectBase.getCanvas("Canvas_HUD").GetComponent<Canvas>().enabled = false;
        }

        public static void showHUD() {
            GUIObjectBase.getCanvas("Canvas_HUD").GetComponent<Canvas>().enabled = true;
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
                        if ( PauseManager.isPausing() )
                            return;
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