using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerSetting : StructBase {
        public Dictionary<Settings.Flag, Setting<bool>> flags { get; set; }

        public PlayerSetting() {
            flags = new Dictionary<Settings.Flag, Setting<bool>>();
            setup();
        }

        public void setup() {
            foreach (KeyValuePair<Settings.Flag, SettingRuleElement<bool>> rule in Settings.FlagSettingRules ) {
                flags[rule.Key] = new Setting<bool>(rule.Value);
            }
        }
    }
}
