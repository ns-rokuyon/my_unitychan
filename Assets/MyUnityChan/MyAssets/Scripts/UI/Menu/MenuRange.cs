using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;

namespace MyUnityChan {
    public class MenuRange : GUIObjectBase, ISelectHandler, IDeselectHandler {
        public Text value_text;

        public Slider slider { get; protected set; }
        public Settings.Range key { get; set; }
        private float _min, _max, _default_value;

        public float min {
            get {
                return _min;
            }
            set {
                _min = value;
                slider.minValue = value;
            }
        }

        public float max {
            get {
                return _max;
            }
            set {
                _max = value;
                slider.maxValue = value;
            }
        }

        public float default_value {
            get {
                return _default_value;
            }
            set {
                _default_value = value;
                slider.value = value;
            }
        }

        public float x {
            get {
                return slider.value;
            }
        }

        void Awake() {
            slider = GetComponent<Slider>();
            setupSoundPlayer();
        }

        void Start() {
            // Value text updater
            this.ObserveEveryValueChanged(_ => x)
                .Subscribe(x => {
                    float _x = approximate(x);
                    changeTo(_x);
                    value_text.text = _x.ToString();
                });
        }

        public void changeTo(float v) {
            SettingManager.set(key, v);
        }

        public float approximate(float v) {
            return Mathf.Round(v * 100) / 100.0f;
        }

        public void OnSelect(BaseEventData event_data) {
            se(Const.ID.SE.BUTTON_SELECT);
        }

        public void OnDeselect(BaseEventData event_data) {
        }
    }
}