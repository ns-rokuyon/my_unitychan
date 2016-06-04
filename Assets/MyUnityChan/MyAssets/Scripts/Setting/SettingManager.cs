using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class SettingManager : SingletonObjectBase<SettingManager> {
        public GameObject scroll_content;
        private EventSystem es;
        private PlayerManager pm;

        private Dictionary<Setting<bool>, GameObject> flag_setting_objects;

        public Settings.Category focus_category { get; set; }

        void Awake() {
            flag_setting_objects = new Dictionary<Setting<bool>, GameObject>();
            es = EventSystem.current;
            focus_category = Settings.Category.GENERAL;
        }

        void Start() {
            pm = GameStateManager.getPlayer().manager;

            // Instantiate flag settings
            foreach ( var setting in pm.status.setting.flags ) {
                GameObject toggle = PrefabInstantiater.create(Const.Prefab.UI["TOGGLE"]);
                toggle.GetComponentInChildren<Text>().text = setting.Value.title.get();
                toggle.setParent(scroll_content);
                toggle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                toggle.GetComponent<MenuToggle>().setFlagKey(setting.Key);
                if ( setting.Value.value == true ) {
                    toggle.GetComponent<Toggle>().isOn = true;
                }

                flag_setting_objects.Add(setting.Value, toggle);
            }

            // Instantiate other settings
            // ...


            // Switch category page
            this.ObserveEveryValueChanged(_ => focus_category)
                .Subscribe(_ => {
                    // flag settings
                    foreach ( var kv in flag_setting_objects ) {
                        if ( kv.Key.category == focus_category )
                            kv.Value.SetActive(true);
                        else
                            kv.Value.SetActive(false);
                    }

                    // other settings
                    // ...
                });
        }

        public static void changeCategory(Settings.Category cate) {
            Instance.focus_category = cate;
        }

        public static void set(Settings.Flag key, bool v) {
            Instance.pm.status.setting.flags[key].value = v;
        }

        public static bool get(Settings.Flag key) {
            return Instance.pm.status.setting.flags[key].value;
        }
    }
}
