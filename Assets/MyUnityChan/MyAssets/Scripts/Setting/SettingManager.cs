using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {
    public class SettingManager : SingletonObjectBase<SettingManager> {
        public GameObject scroll_content;
        private List<GameObject> setting_objects;
        private EventSystem es;
        private PlayerManager pm;

        void Awake() {
            setting_objects = new List<GameObject>();
            es = EventSystem.current;
        }

        void Start() {
            pm = GameStateManager.getPlayer().manager;

            foreach ( var setting in pm.status.setting.flags ) {
                GameObject toggle = PrefabInstantiater.create(Const.Prefab.UI["TOGGLE"]);
                //toggle.GetComponent<RectTransform>().position = new Vector3(0, 300, 0);
                toggle.GetComponentInChildren<Text>().text = setting.Value.title.get();
                toggle.setParent(scroll_content);
                toggle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                toggle.GetComponent<MenuToggle>().setFlagKey(setting.Key);
                if ( setting.Value.value == true ) {
                    toggle.GetComponent<Toggle>().isOn = true;
                }

                setting_objects.Add(toggle);
            }
        }

        public static void set(Settings.Flag key, bool v) {
            Instance.pm.status.setting.flags[key].value = v;
        }

        public static bool get(Settings.Flag key) {
            return Instance.pm.status.setting.flags[key].value;
        }
    }
}
