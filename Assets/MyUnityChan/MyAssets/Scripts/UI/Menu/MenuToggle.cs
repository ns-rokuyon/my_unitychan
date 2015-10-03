using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class MenuToggle : GUIObjectBase, ISelectHandler {

        private Toggle toggle;
        private Settings.Flag flag_key;

        void Awake() {
            toggle = gameObject.GetComponent<Toggle>();
        }

        public void setFlagKey(Settings.Flag k) {
            flag_key = k;
        }

        public void OnSelect(BaseEventData event_data) {
            iTween.ScaleTo(this.gameObject, iTween.Hash("x", 1.05f, "y", 1.05f, "z", 1.05f, "time", 0.05f));
            iTween.ScaleTo(this.gameObject, iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.05f, "delay", 0.1f));
        }

        public void changeSetting(bool b) {
            SettingManager.Instance.set(flag_key, toggle.isOn);
        }
    }

}
