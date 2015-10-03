using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {
    public class SettingManager : SingletonObjectBase<SettingManager> {
        public GameObject canvas_object;
        public int setting_objects_top_to_top = 300;
        private GameObject scroll_content;
        private List<GameObject> setting_objects;
        private Dictionary<Settings.Flag, Setting<bool>> flag_settings;
        private EventSystem es;

        void Awake() {
            setting_objects = new List<GameObject>();
            flag_settings = new Dictionary<Settings.Flag, Setting<bool>>();
            scroll_content = canvas_object.transform.FindChild("Scroll List/Content").gameObject;
            es = EventSystem.current;

            foreach (KeyValuePair<Settings.Flag, SettingRuleElement<bool>> rule in Settings.FlagSettingRules ) {
                flag_settings[rule.Key] = new Setting<bool>(
                    rule.Value.default_value,
                    GameText.text(rule.Value.title_jp, rule.Value.title_en),
                    GameText.text(rule.Value.desc_jp, rule.Value.desc_en)
                    );
            }

            foreach ( var setting in flag_settings ) {
                GameObject toggle = PrefabInstantiater.create(Const.Prefab.UI["TOGGLE"]);
                toggle.GetComponent<RectTransform>().position = new Vector3(0, 300, 0);
                toggle.GetComponentInChildren<Text>().text = setting.Value.title.get();
                toggle.setParent(scroll_content);
                toggle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                toggle.GetComponent<MenuToggle>().setFlagKey(setting.Key);
                if ( setting.Value.value == true ) {
                    toggle.GetComponent<Toggle>().isOn = true;
                }

                setting_objects.Add(toggle);
            }

            canvas_object.SetActive(false);
        }

        public void set(Settings.Flag key, bool v) {
            flag_settings[key].value = v;
        }

        public bool get(Settings.Flag key) {
            return flag_settings[key].value;
        }

        public void enter() {
            canvas_object.SetActive(true);
            es.SetSelectedGameObject(setting_objects.FirstOrDefault());
        }

        public void quit() {
            canvas_object.SetActive(false);
            es.SetSelectedGameObject(null);
        }
    }
}
