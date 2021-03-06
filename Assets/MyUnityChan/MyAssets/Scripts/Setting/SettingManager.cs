﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using TMPro;

namespace MyUnityChan {
    public class SettingManager : SingletonObjectBase<SettingManager> {
        public GameObject scroll_content;
        private EventSystem es;
        private PlayerManager pm;

        private Dictionary<Setting<bool>, GameObject> flag_setting_objects;
        private Dictionary<SettingSelect, GameObject> select_setting_objects;
        private Dictionary<SettingRange, GameObject> range_setting_objects;

        public Settings.Category focus_category { get; set; }
        public Dictionary<Settings.Category, GameObject> corresponding_parent_buttons { get; protected set; }
        public Dictionary<Settings.Flag, System.IDisposable> flag_setting_callbacks { get; set; }
        public bool focus_on_content { get; protected set; }
        public bool setup_done { get; set; }

        void Awake() {
            setup_done = false;
            flag_setting_objects = new Dictionary<Setting<bool>, GameObject>();
            select_setting_objects = new Dictionary<SettingSelect, GameObject>();
            range_setting_objects = new Dictionary<SettingRange, GameObject>();
            es = EventSystem.current;
            focus_category = Settings.Category.GENERAL;

            corresponding_parent_buttons = new Dictionary<Settings.Category, GameObject>();
            flag_setting_callbacks = new Dictionary<Settings.Flag, System.IDisposable>();
        }

        void Start() {
            pm = GameStateManager.getPlayer().manager;

            // Instantiate flag settings
            foreach ( var setting in pm.status.setting.flags ) {
                GameObject toggle = PrefabInstantiater.create(Const.Prefab.UI["TOGGLE"]);
                RectTransform rt = toggle.GetComponent<RectTransform>();
                toggle.GetComponentInChildren<TextMeshProUGUI>().text = setting.Value.title.get();
                toggle.setParent(scroll_content);
                toggle.GetComponent<MenuToggle>().setFlagKey(setting.Key);
                rt.localScale = new Vector3(1, 1, 1);
                rt.localPosition = rt.localPosition.changeZ(0.0f);
                if ( setting.Value.value == true ) {
                    toggle.GetComponent<Toggle>().isOn = true;
                }

                flag_setting_objects.Add(setting.Value, toggle);
            }

            // Instantiate select settings
            foreach ( var setting in pm.status.setting.selects ) {
                GameObject dropdown = PrefabInstantiater.create(Const.Prefab.UI["DROPDOWN"]);
                RectTransform rt = dropdown.GetComponent<RectTransform>();
                dropdown.GetComponentInChildren<TextMeshProUGUI>().text = setting.Value.title.get();
                dropdown.setParent(scroll_content);
                dropdown.GetComponent<MenuDropdownButton>().key = setting.Key;
                dropdown.GetComponent<MenuDropdownButton>().created_by_settingmanager = true;
                dropdown.GetComponentInChildren<Dropdown>().options.Clear();
                rt.localScale = new Vector3(1, 1, 1);
                rt.localPosition = rt.localPosition.changeZ(0.0f);
                foreach ( var option in setting.Value.item_texts ) {
                    dropdown.GetComponentInChildren<Dropdown>().options.Add(new Dropdown.OptionData(option.get()));
                }

                select_setting_objects.Add(setting.Value, dropdown);
            }

            // Instantiate range settings
            foreach ( var setting in pm.status.setting.ranges ) {
                GameObject obj = PrefabInstantiater.create(Const.Prefab.UI["RANGE"]);
                MenuRange range = obj.GetComponent<MenuRange>();
                RectTransform rt = obj.GetComponent<RectTransform>();
                obj.GetComponentInChildren<TextMeshProUGUI>().text = setting.Value.title.get();
                obj.setParent(scroll_content);

                range.key = setting.Key;
                range.min = setting.Value.min;
                range.max = setting.Value.max;
                range.default_value = setting.Value.default_value;

                rt.localScale = new Vector3(1, 1, 1);
                rt.localPosition = rt.localPosition.changeZ(0.0f);

                range_setting_objects.Add(setting.Value, obj);
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

                    // range settings
                    foreach ( var kv in range_setting_objects ) {
                        if ( kv.Key.category == focus_category )
                            kv.Value.SetActive(true);
                        else
                            kv.Value.SetActive(false);
                    }

                    // other settings
                    // ...


                    // Explicit navigations
                    List<Selectable> selectables = getActiveSelectables();
                    for ( int i = 0; i < selectables.Count; i++ ) {
                        var next = i + 1 >= selectables.Count ? selectables[0] : selectables[i + 1];
                        var prev = i - 1 < 0 ? selectables[selectables.Count - 1] : selectables[i - 1];
                        var navigation = selectables[i].navigation;
                        // Set navigations
                        navigation.selectOnDown = next;
                        navigation.selectOnUp = prev;
                        if ( corresponding_parent_buttons.ContainsKey(focus_category) ) {
                            var corresponding = corresponding_parent_buttons[focus_category].GetComponent<Selectable>();
                            //navigation.selectOnLeft = corresponding;

                            var conav = corresponding.navigation;
                            conav.selectOnRight = selectables[0];
                            corresponding.navigation = conav;
                        }
                        selectables[i].navigation = navigation;
                    }
                });

            // If LANG is changed, reset label text for all button
            this.ObserveEveryValueChanged(_ => get<Const.Language>(Settings.Select.LANG))
                .Subscribe(_ => resetLanguage());

            // Updater of focus_on_content variable
            this.UpdateAsObservable()
                .Subscribe(_ => {
                    var current_obj = es.currentSelectedGameObject;
                    if ( !current_obj ) {
                        focus_on_content = false;
                        return;
                    }
                    var current = current_obj.GetComponent<Selectable>();
                    if ( !current ) {
                        focus_on_content = false;
                        return;
                    }
                    focus_on_content = getActiveSelectables().Contains(current);
                })
                .AddTo(this);

            // Controller
            this.UpdateAsObservable()
                .Where(_ => focus_on_content)
                .Subscribe(_ => {
                    if ( GameStateManager.Instance.player_manager.controller.keyCancel() ) {
                        // Back to sidebar menu from focused content
                        corresponding_parent_buttons[focus_category].GetComponent<Selectable>().Select();
                    }
                })
                .AddTo(this);

            setup_done = true;
        }

        public List<Selectable> getActiveSelectables() {
            return scroll_content.GetComponentsInChildren<Selectable>().ToList();
        }

        public void addCorrespondingParentElement(Settings.Category key, GameObject value) {
            corresponding_parent_buttons.Add(key, value);
        }

        private void resetLanguage() {
            if ( flag_setting_objects == null ) return;
            if ( select_setting_objects == null ) return;

            foreach ( var kv in flag_setting_objects ) {
                kv.Value.GetComponentInChildren<TextMeshProUGUI>().text = kv.Key.title.get();
            }
            foreach ( var kv in select_setting_objects ) {
                kv.Value.GetComponentInChildren<TextMeshProUGUI>().text = kv.Key.title.get();
            }
            foreach ( var kv in range_setting_objects ) {
                kv.Value.GetComponentInChildren<TextMeshProUGUI>().text = kv.Key.title.get();
            }
        }

        public static bool isSetupDone() {
            return self().setup_done;
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

        // Set range setting value
        public static void set(Settings.Range key, float v) {
            Instance.pm.status.setting.ranges[key].value = v;
        }

        // Get flag setting value
        public static bool get(Settings.Flag key) {
            return Instance.pm.status.setting.flags[key].value;
        }

        // Get select setting value
        public static T get<T>(Settings.Select key) {
            return Instance.pm.status.setting.selects[key].selected<T>();
        }

        // Get range setting value
        public static float get(Settings.Range key) {
            return Instance.pm.status.setting.ranges[key].value;
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
