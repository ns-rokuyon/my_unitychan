using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class MenuToggle : GUIObjectBase, ISelectHandler, IDeselectHandler {

        private Toggle toggle;
        private Image frame;
        private Settings.Flag flag_key;

        void Awake() {
            toggle = gameObject.GetComponent<Toggle>();
            frame = gameObject.GetComponentInChildren<Image>();
        }

        void Update() {
        }

        public void setFlagKey(Settings.Flag k) {
            flag_key = k;
        }

        public void OnSelect(BaseEventData event_data) {
            frame.color = Color.green;
        }

        public void OnDeselect(BaseEventData event_data) {
            frame.color = Color.white;
        }

        public void changeSetting(bool b) {
            SettingManager.Instance.set(flag_key, toggle.isOn);
        }
    }

}
