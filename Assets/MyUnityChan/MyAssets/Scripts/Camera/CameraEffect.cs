using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using UnityStandardAssets.ImageEffects;
using UniRx;
using DG.Tweening;
using UnityEngine.PostProcessing;

namespace MyUnityChan {
    public class CameraEffect : ObjectBase {
        // Post-processing(Unity >= 5.6)
        public PostProcessingBehaviour post_processing { get; protected set; }

        public BloomModel bloom { get; protected set; }
        public DepthOfFieldModel dof { get; protected set; }
        public MotionBlurModel blur { get; protected set; }
        public VignetteModel vignette { get; protected set; }

        // Fadeout/in
        public FadeImage fade { get; protected set; }
        public IDisposable fader { get; protected set; }

        // Flag
        public bool _restore { get; set; }

        void Awake() {
            post_processing = GetComponent<PostProcessingBehaviour>();

            bloom = post_processing.profile.bloom;
            dof = post_processing.profile.depthOfField;
            blur = post_processing.profile.motionBlur;
            vignette = post_processing.profile.vignette;

            fade = GUIObjectBase.getCanvas(Const.Canvas.FADE_CANVAS).GetComponent<FadeImage>();
            fader = null;
        }

        public void restore() {
            _restore = true;
        }

        public void setPauseMenuEffect() {
            var _dof_settings = dof.settings;
            var _vignetting_settings = vignette.settings;

            var dof_settings = dof.settings;
            dof_settings.aperture = 1.0f;
            dof.settings = dof_settings;

            var vignette_settings = vignette.settings;
            vignette_settings.intensity = 0.4f;
            vignette.settings = vignette_settings;

            // Restore
            this.ObserveEveryValueChanged(_ => _restore)
                .Where(f => f)
                .First()
                .Subscribe(_ => {
                    dof.settings = _dof_settings;
                    vignette.settings = _vignetting_settings;
                    _restore = false;
                });
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