using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.PostProcessing;
using System.Linq;

namespace MyUnityChan {
    public class CameraEffect : ObjectBase {

        [Serializable]
        public class GameStatePostProcessingDef : KV<Const.ID.GameState, PostProcessingProfile> {
            public GameStatePostProcessingDef(Const.ID.GameState state, PostProcessingProfile prof) : base(state, prof) {
            }

            public Const.ID.GameState state { get { return key; } }
            public PostProcessingProfile profile { get { return value; } }
        }

        [Serializable]
        public class PostProcessingDef : KV<Const.ID.PostProcessingType, PostProcessingProfile> {
            public PostProcessingDef(Const.ID.PostProcessingType type, PostProcessingProfile prof) : base(type, prof) {
            }

            public Const.ID.PostProcessingType type { get { return key; } }
            public PostProcessingProfile profile { get { return value; } }
        }

        [SerializeField]
        private List<PostProcessingDef> post_processing_defs;

        [SerializeField]
        private List<GameStatePostProcessingDef> gamestate_post_processing_defs;

        private Dictionary<Const.ID.PostProcessingType, PostProcessingProfile> _post_processing_defs { get; } =
            new Dictionary<Const.ID.PostProcessingType, PostProcessingProfile>();

        private Dictionary<Const.ID.GameState, PostProcessingProfile> _gamestate_post_processing_defs { get; } =
            new Dictionary<Const.ID.GameState, PostProcessingProfile>();

        // Post-processing(Unity >= 5.6)
        public PostProcessingBehaviour post_processing { get; protected set; }

        // Fadeout/in
        public FadeImage fade { get; protected set; }
        public IDisposable fader { get; protected set; }

        private IDisposable gamestate_post_processing_profiler;
        private System.Action restorer;

        void Awake() {
            post_processing = GetComponent<PostProcessingBehaviour>();

            post_processing_defs.ForEach(def => {
                _post_processing_defs[def.type] = def.profile;
            });
            gamestate_post_processing_defs.ForEach(def => {
                _gamestate_post_processing_defs[def.state] = def.profile;
            });

            fade = GUIObjectBase.getCanvas(Const.Canvas.FADE_CANVAS).GetComponent<FadeImage>();
            fader = null;
        }

        void Start() {
            gamestate_post_processing_profiler = GameStateManager.StateStream.Subscribe(state => {
                var profile = getGameStatePostProcessingProfileOf(state);
                if ( profile )
                    post_processing.profile = profile;
            })
            .AddTo(this);
        }

        public PostProcessingProfile getPostProcessingProfileOf(Const.ID.PostProcessingType type) {
            if ( !_post_processing_defs.ContainsKey(type) ) {
                return null;
            }
            return _post_processing_defs[type];
        }

        public PostProcessingProfile getGameStatePostProcessingProfileOf(Const.ID.GameState state) {
            if ( !_gamestate_post_processing_defs.ContainsKey(state) ) {
                return null;
            }
            return _gamestate_post_processing_defs[state];
        }

        public void changePostProcessing(Const.ID.PostProcessingType type) {
            var profile = getPostProcessingProfileOf(type);
            if ( profile == null )
                return;

            var prev_profile = post_processing.profile;
            post_processing.profile = profile;

            restorer = () => post_processing.profile = prev_profile;
        }

        public void restorePostProcessing() {
            if ( restorer == null ) {
                return;
            }
            restorer();
            restorer = null;
        }

        public void disableFadeCanvas() {
            var canvas = GUIObjectBase.getCanvas("Canvas_Fade");
            if ( canvas )
                canvas.SetActive(false);
        }

        public void enableFadeCanvas() {
            var canvas = GUIObjectBase.getCanvas("Canvas_Fade");
            if ( canvas )
                canvas.SetActive(true);
        }

        public void fadeOut(int frame) {
            fade.Range = 0.0f;
            if ( fader != null )
                fader.Dispose();
            float d = 1.0f / (float)frame;
            fader = Observable.IntervalFrame(1)
                .Take(frame)
                .Subscribe(t => {
                    fade.Range += d;
                    if ( fade.Range > 1.0f )
                        fade.Range = 1.0f;
                })
                .AddTo(this);
        }

        public void fadeIn(int frame) {
            fade.Range = 1.0f;
            if ( fader != null )
                fader.Dispose();
            float d = 1.0f / (float)frame;
            fader = Observable.IntervalFrame(1)
                .Take(frame)
                .Subscribe(t => {
                    fade.Range -= d;
                    if ( fade.Range < 0 )
                        fade.Range = 0.0f;
                })
                .AddTo(this);
        }
    }
}