using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {
    public class SettingManager : SingletonObjectBase<SettingManager> {
        public GameObject canvas_object;
        private GameObject scroll_content;
        private Dictionary<Settings.Flag, Setting<bool>> flag_settings;

        void Awake() {
            flag_settings = new Dictionary<Settings.Flag, Setting<bool>>();

            scroll_content = canvas_object.transform.FindChild("Scroll List/Content").gameObject;

            // TODO
            Setting<bool> debug_window_setting = new Setting<bool>(false, GameText.text("show debug window"), null);
            flag_settings[Settings.Flag.SHOW_DEBUG_WINDOW] = debug_window_setting;

            foreach ( var setting in flag_settings ) {
                GameObject toggle = PrefabInstantiater.create(Const.Prefab.UI["TOGGLE"]);
                toggle.GetComponent<RectTransform>().position = new Vector3(0, 300, 0);
                toggle.GetComponentInChildren<Text>().text = setting.Value.title.get();
                toggle.setParent(scroll_content);
                toggle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

            canvas_object.SetActive(false);
        }

        public void set(Settings.Flag key, bool v) {
            flag_settings[key].value = v;
        }

        public void enter() {
            canvas_object.SetActive(true);
        }

        public void quit() {
            canvas_object.SetActive(false);
        }
    }
}
