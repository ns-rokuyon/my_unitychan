using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    [System.Serializable]
    public class MenuNavbarButtonPlayerSetting : MenuNavbarButton<Settings.Category> {
        public Button button { get; protected set; }
        public Text text { get; protected set; }
        public Image icon { get; protected set; }

        private Color default_text_color;
        private Color default_icon_color;

        void Start() {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            icon = GetComponentInChildren<Image>();

            button.onClick.AddListener(change);
            SettingManager.Instance.addCorrespondingParentElement(nav, gameObject);

            default_text_color = text.color;
            default_icon_color = icon.color;

            this.UpdateAsObservable()
                .Subscribe(_ => {
                    if ( SettingManager.Instance.focus_on_content ) {
                        button.interactable = SettingManager.Instance.focus_category == nav;
                    }
                    else {
                        button.interactable = true;
                    }
                })
                .AddTo(this);

            this.ObserveEveryValueChanged(_ => button.interactable)
                .Subscribe(interactable => {
                    if ( interactable ) {
                        // Change to normal color
                        text.color = default_text_color;
                        icon.color = default_icon_color;
                    }
                    else {
                        // Change to disabled color
                        text.color = new Color32(200, 200, 200, 128);
                        icon.color = new Color32(200, 200, 200, 128);
                    }
                })
                .AddTo(this);
        }

        public override void change() {
            SettingManager.Instance.focus_category = nav;
        }
    }
}
