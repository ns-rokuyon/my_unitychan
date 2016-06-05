﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    [System.Serializable]
    public class MenuNavbarButtonPlayerSetting : MenuNavbarButton<Settings.Category> {
        private Button button;

        void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(change);
        }

        public override void change() {
            SettingManager.Instance.focus_category = nav;
        }
    }
}
