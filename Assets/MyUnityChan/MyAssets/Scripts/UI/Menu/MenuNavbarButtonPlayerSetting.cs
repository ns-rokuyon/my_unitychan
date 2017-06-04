using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    [System.Serializable]
    public class MenuNavbarButtonPlayerSetting : MenuNavbarButton<Settings.Category> {
        public Button button { get; protected set; }

        void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(change);
            SettingManager.Instance.addCorrespondingParentElement(nav, gameObject);
        }

        public override void change() {
            SettingManager.Instance.focus_category = nav;
        }
    }
}
