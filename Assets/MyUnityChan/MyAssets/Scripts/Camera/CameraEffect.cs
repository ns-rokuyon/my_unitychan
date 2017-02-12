using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityStandardAssets.ImageEffects;
using UniRx;

namespace MyUnityChan {
    public class CameraEffect : ObjectBase {
        public Bloom bloom { get; protected set; }
        public DepthOfField dof { get; protected set; }
        public CameraMotionBlur blur { get; protected set; }
        public VignetteAndChromaticAberration vignette { get; protected set; }

        public bool _restore { get; set; }

        void Awake() {
            bloom = GetComponent<Bloom>();
            dof = GetComponent<DepthOfField>();
            blur = GetComponent<CameraMotionBlur>();
            vignette = GetComponent<VignetteAndChromaticAberration>();
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
    }
}