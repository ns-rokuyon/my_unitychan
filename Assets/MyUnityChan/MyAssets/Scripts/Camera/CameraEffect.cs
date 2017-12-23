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
        public class PostProcessingDef : KV<Const.ID.PostProcessingType, PostProcessingProfile> {
            public PostProcessingDef(Const.ID.PostProcessingType type, PostProcessingProfile prof) : base(type, prof) {
            }

            public Const.ID.PostProcessingType type { get { return key; } }
            public PostProcessingProfile profile { get { return value; } }
        }

        [SerializeField]
        public List<PostProcessingDef> post_processing_defs;

        public Dictionary<Const.ID.PostProcessingType, PostProcessingProfile> _post_processing_defs;

        // Post-processing(Unity >= 5.6)
        public PostProcessingBehaviour post_processing { get; protected set; }

        // Fadeout/in
        public FadeImage fade { get; protected set; }
        public IDisposable fader { get; protected set; }

        // Flag
        public bool _restore { get; set; }

        void Awake() {
            post_processing = GetComponent<PostProcessingBehaviour>();

            _post_processing_defs = new Dictionary<Const.ID.PostProcessingType, PostProcessingProfile>();
            post_processing_defs.ForEach(def => {
                _post_processing_defs[def.type] = def.profile;
            });

            fade = GUIObjectBase.getCanvas(Const.Canvas.FADE_CANVAS).GetComponent<FadeImage>();
            fader = null;
        }

        void Start() {
        }

        public void restore() {
            _restore = true;
        }

        public PostProcessingProfile getPostProcessingProfileOf(Const.ID.PostProcessingType type) {
            if ( !_post_processing_defs.ContainsKey(type) ) {
                return null;
            }
            return _post_processing_defs[type];
        }

        public void setPauseMenuEffect() {
            var profile = getPostProcessingProfileOf(Const.ID.PostProcessingType.PAUSE);
            if ( profile == null )
                return;

            var current_profile = post_processing.profile;
            post_processing.profile = profile;

            // Restore
            this.ObserveEveryValueChanged(_ => _restore)
                .Where(f => f)
                .First()
                .Subscribe(_ => {
                    post_processing.profile = current_profile;
                    _restore = false;
                });
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