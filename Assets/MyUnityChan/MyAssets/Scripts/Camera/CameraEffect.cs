using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using UnityStandardAssets.ImageEffects;
using UniRx;
using DG.Tweening;

namespace MyUnityChan {
    public class CameraEffect : ObjectBase {
        // Image effects
        public Bloom bloom { get; protected set; }
        public DepthOfField dof { get; protected set; }
        public CameraMotionBlur blur { get; protected set; }
        public VignetteAndChromaticAberration vignette { get; protected set; }

        // Fadeout/in
        public FadeImage fade { get; protected set; }
        public IDisposable fader { get; protected set; }

        // Flag
        public bool _restore { get; set; }

        void Awake() {
            bloom = GetComponent<Bloom>();
            dof = GetComponent<DepthOfField>();
            blur = GetComponent<CameraMotionBlur>();
            vignette = GetComponent<VignetteAndChromaticAberration>();

            fade = GUIObjectBase.getCanvas(Const.Canvas.FADE_CANVAS).GetComponent<FadeImage>();
            fader = null;
        }

        public void restore() {
            _restore = true;
        }

        public void setPauseMenuEffect() {
            float _aperture = dof.aperture;
            float _vignetting = vignette.intensity;

            dof.aperture = 1.0f;
            vignette.intensity = 0.4f;

            // Restore
            this.ObserveEveryValueChanged(_ => _restore)
                .Where(f => f)
                .First()
                .Subscribe(_ => {
                    dof.aperture = _aperture;
                    vignette.intensity = _vignetting;
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