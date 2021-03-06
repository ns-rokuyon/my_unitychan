﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class GameStateManager : SingletonObjectBase<GameStateManager> {
        public PlayerDemo showcase;

        public PlayerManager player_manager { get; set; }

        private RandomNumberGenerator rng { get; } = new RandomNumberGenerator();
        private ReactiveProperty<Const.ID.GameState> state { get; } = new ReactiveProperty<Const.ID.GameState>(Const.ID.GameState.NO_SET);

        public Const.Language language {
            get {
                return SettingManager.isSetupDone() ? SettingManager.get<Const.Language>(Settings.Select.LANG) : Const.Language.JP;
            }
        }

        public static ReadOnlyReactiveProperty<Const.ID.GameState> StateStream {
            get { return self().state.ToReadOnlyReactiveProperty(); }
        }

        public static bool gameover {
            get { return Instance.player_manager != null ? Instance.player_manager.gameover : false; }
        }

        public static PlayerManager pm {
            get { return Instance.player_manager; }
        }

        public static PlayerController controller {
            get { return Instance.player_manager.controller as PlayerController; }
        }

        public static float fps {
            get { return 1.0f / Time.deltaTime; }
        }

        public static int approximatedFps {
            get { return (int)fps; }
        }

        public static RandomNumberGenerator RNG {
            get { return Instance.rng; }
        }

        public static Const.ID.GameState now() {
            return self().state.Value;
        }

        public static bool isLoadingInBackground() {
            return AssetBundleManager.isNowLoading();
        }

        public static void change(Const.ID.GameState st) {
            self().state.Value = st;
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

        void Start() {
            change(Const.ID.GameState.MAIN);

            this.UpdateAsObservable()
                .Subscribe(_ => watchStateTransition())
                .AddTo(this);
        }


        private void watchStateTransition() {
            Controller controller = getPlayer().getController();
            switch ( state.Value ) { 
                case Const.ID.GameState.MAIN: 
                    if ( controller.keyPause() ) {
                        /* 
                         * State: MAIN -> MENU 
                         * Trigger: pause key was pressed
                         */
                        if ( PauseManager.isPausing() )
                            return;
                        change(Const.ID.GameState.PAUSE_MENU);
                        PauseManager.Instance.pause(true);
                        MenuManager.Instance.enter();
                    }
                    else {
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                    break;
                case Const.ID.GameState.PAUSE_MENU:
                    if ( controller.keyPause() ) {
                        /*
                         * State: MENU -> MAIN
                         * Trigger: pause key was pressed
                         */
                        change(Const.ID.GameState.MAIN);
                        MenuManager.Instance.quit();
                        PauseManager.Instance.pause(false);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public class RandomNumberGenerator : StructBase {
        public RandomNumberGenerator() {
        }

        public float value {
            get { return Random.value; }
        }

        public void setSeed(int seed) {
            Random.InitState(seed);
        }

        public bool prob(float p) {
            return prob(p, value);
        }

        public bool prob(float p, float rand) {
            if ( p <= 0.0f )
                return false;
            if ( p >= 1.0f )
                return true;
            if ( p <= rand )
                return false;
            return true;
        }

        public T prob<S, T>(List<S> candidates) where S : KP<T>  {
            if ( candidates.Select(c => c.prob).Sum() > (float)candidates.Count ) {
                throw new System.Exception("Invalid prob");
            }

            var cs = candidates.OrderBy(c => c.prob);

            float rand = value;
            foreach( var c in cs ) {
                if ( prob(c.prob, rand) ) {
                    return c.key;
                }
            }
            return default(T);
        }
    }
}