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
                flags[rule.Key] = new Setting<bool>(
                    rule.Value.default_value,
                    GameText.text(rule.Value.title_jp, rule.Value.title_en),
                    GameText.text(rule.Value.desc_jp, rule.Value.desc_en)
                    );
            }
        }
    }
}
