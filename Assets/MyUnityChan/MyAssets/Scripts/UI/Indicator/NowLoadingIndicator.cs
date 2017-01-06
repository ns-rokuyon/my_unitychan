using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using EnergyBarToolkit;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class NowLoadingIndicator : GUIObjectBase {
        private List<EnergyBar> bars;
        private Dictionary<EnergyBar, FilledRendererUGUI> renderers;
        private Dictionary<EnergyBar, int> speeds;
        private Text text;

        private int alpha_min;
        private int alpha_max;

        void Awake() {
            renderers = new Dictionary<EnergyBar, FilledRendererUGUI>();
            speeds = new Dictionary<EnergyBar, int>();
            bars = new List<EnergyBar>(GetComponentsInChildren<EnergyBar>());
            bars.ForEach(bar => {
                renderers.Add(bar, bar.GetComponent<FilledRendererUGUI>());
                speeds.Add(bar, (int)UnityEngine.Random.Range(1, 2));
            });
            text = GetComponentInChildren<Text>();
            alpha_min = 20;
            alpha_max = 240;
        }

        void Start() {
            // Update
            Observable.IntervalFrame(2)
                .Where(_ => GameStateManager.isLoadingInBackground())
                .Subscribe(_ => onDisplay());

            // Close
            this.ObserveEveryValueChanged(_ => GameStateManager.isLoadingInBackground())
                .Subscribe(flag => {
                    if ( !flag )
                        Destroy(gameObject);
                });
        }

        protected void onDisplay() {
            // Rotation
            bars.ForEach(bar => {
                if ( bar.valueCurrent >= bar.valueMax ) {
                    bar.valueCurrent = bar.valueMin;
                    if ( renderers[bar].radialOffset >= 1.0f ) {
                        renderers[bar].radialOffset = -1.0f;
                    }
                    else {
                        renderers[bar].radialOffset += 0.1f;
                    }
                    speeds[bar] = (int)UnityEngine.Random.Range(1, 4);
                }
                else {
                    bar.valueCurrent += speeds[bar];
                }
            });

            // Text
            var alpha = (alpha_min + Mathf.PingPong(Time.frameCount * 2, alpha_max - alpha_min)) / 255.0f;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }
}
