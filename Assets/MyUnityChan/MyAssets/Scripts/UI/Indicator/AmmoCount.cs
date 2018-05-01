using UnityEngine;
using System.Collections;
using System;
using UniRx;
using TMPro;

namespace MyUnityChan {
    public class AmmoCount : GUIObjectBase {
        [SerializeField]
        private TextMeshProUGUI current_count;

        [SerializeField]
        private TextMeshProUGUI max_count;

        private IDisposable current_count_updater;
        private IDisposable max_count_updater;

        private readonly float WIDTH_PER_CHAR = 30.0f;

        public void connect(ReactiveProperty<int> current_prop, ReactiveProperty<int> max_prop) {
            connect(current_prop.ToReadOnlyReactiveProperty(), max_prop.ToReadOnlyReactiveProperty());
        }

        public void connect(ReadOnlyReactiveProperty<int> current_prop, ReadOnlyReactiveProperty<int> max_prop) {
            current_count_updater = current_prop.Subscribe(onUpdateCurrentCount);
            max_count_updater = max_prop.Subscribe(onUpdateMaxCount);
        }

        private void onUpdateCurrentCount(int count) {
            current_count.text = count.ToString();

            float height = current_count.rectTransform.sizeDelta.y;
            int n_char = 1 + (count > 0 ? (int)Mathf.Log10(count) : 0);
            current_count.rectTransform.sizeDelta = new Vector2(WIDTH_PER_CHAR * n_char, height);
        }

        private void onUpdateMaxCount(int max) {
            max_count.text = max.ToString();
        }
    }
}
