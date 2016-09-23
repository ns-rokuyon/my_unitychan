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
        private Dictionary<SettingSelect, GameObject> select_setting_objects;

        public Settings.Category focus_category { get; set; }
        public Dictionary<Settings.Flag, System.IDisposable> flag_setting_callbacks { get; set; }

        void Awake() {
            flag_setting_objects = new Dictionary<Setting<bool>, GameObject>();
            select_setting_objects = new Dictionary<SettingSelect, GameObject>();
            es = EventSystem.current;
            focus_category = Settings.Category.GENERAL;

            flag_setting_callbacks = new Dictionary<Settings.Flag, System.IDisposable>();
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

            // Instantiate select settings
            foreach ( var setting in pm.status.setting.selects ) {
                GameObject dropdown = PrefabInstantiater.create(Const.Prefab.UI["DROPDOWN"]);
                dropdown.GetComponentInChildren<Text>().text = setting.Value.title.get();
                dropdown.setParent(scroll_content);
                dropdown.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                dropdown.GetComponent<MenuDropdownButton>().key = setting.Key;
                dropdown.GetComponent<MenuDropdownButton>().created_by_settingmanager = true;
                dropdown.GetComponentInChildren<Dropdown>().options.Clear();
                foreach ( var option in setting.Value.item_texts ) {
                    dropdown.GetComponentInChildren<Dropdown>().options.Add(new Dropdown.OptionData(option.get()));
                }

                select_setting_objects.Add(setting.Value, dropdown);
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

                    // select settings
                    foreach ( var kv in select_setting_objects ) {
                        if ( kv.Key.category == focus_category )
                            kv.Value.SetActive(true);
                        else
                            kv.Value.SetActive(false);
                    }

                    // other settings
                    // ...
                });

            // If LANG is changed, reset label text for all button
            this.ObserveEveryValueChanged(_ => get<Const.Language>(Settings.Select.LANG))
                .Subscribe(_ => resetLanguage());
        }

        private void resetLanguage() {
            if ( flag_setting_objects == null ) return;
            if ( select_setting_objects == null ) return;

            foreach ( var kv in flag_setting_objects ) {
                kv.Value.GetComponentInChildren<Text>().text = kv.Key.title.get();
            }
            foreach ( var kv in select_setting_objects ) {
                kv.Value.GetComponentInChildren<Text>().text = kv.Key.title.get();
            }
        }

        public static void changeCategory(Settings.Category cate) {
            Instance.focus_category = cate;
        }

        // Set flag setting value
        public static void set(Settings.Flag key, bool v) {
            Instance.pm.status.setting.flags[key].value = v;
        }

        // Set select setting value
        public static void set(Settings.Select key, int v) {
            Instance.pm.status.setting.selects[key].select(v);
        }

        // Get flag setting value
        public static bool get(Settings.Flag key) {
            return Instance.pm.status.setting.flags[key].value;
        }

        // Get select setting value
        public static T get<T>(Settings.Select key) {
            return Instance.pm.status.setting.selects[key].selected<T>();
        }

        // Add callback which invokes 'func(bool flag)' when it's flag changes
        public static void setCallback(Settings.Flag key, System.Action<bool> func) {
            if ( Instance.flag_setting_callbacks.ContainsKey(key) ) return;

            var o = Instance.ObserveEveryValueChanged(_ => {
                    try {
                        return get(key);
                    }
                    catch ( System.NullReferenceException e ) {
                        // TODO
                        return false;
                    }
                })
                .Subscribe(b => func(b))
                .AddTo(Instance.gameObject);
            Instance.flag_setting_callbacks.Add(key, o);
        }
    }
}
