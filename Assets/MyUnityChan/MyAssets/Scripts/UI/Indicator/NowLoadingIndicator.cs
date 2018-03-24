using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using TMPro;
using DG.Tweening;

namespace MyUnityChan {

    public class NowLoadingIndicator : GUIObjectBase {
        private TextMeshProUGUI text;
        private Tween tween;

        private int alpha_min;
        private int alpha_max;

        void Awake() {
            text = GetComponentInChildren<TextMeshProUGUI>();
            alpha_min = 20;
            alpha_max = 240;
        }

        void Start() {
            if ( tween == null ) {
                var seq = DOTween.Sequence()
                    .Append(text.DOFade(alpha_min, 1.0f))
                    .Append(text.DOFade(alpha_max, 1.0f))
                    .SetLoops(-1);
                tween = seq;
                seq.Play();
            }

            // Update
            Observable.IntervalFrame(2)
                .Where(_ => GameStateManager.isLoadingInBackground())
                .Subscribe(_ => onDisplay());

            // Close
            this.ObserveEveryValueChanged(_ => GameStateManager.isLoadingInBackground())
                .Subscribe(flag => {
                    if ( !flag )
                        Destroy(gameObject);
                    if ( tween != null )
                        tween.Kill();
                });
        }

        protected void onDisplay() {
        }
    }
}
