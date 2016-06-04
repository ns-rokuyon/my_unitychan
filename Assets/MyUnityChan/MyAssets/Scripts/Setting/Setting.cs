using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Setting<T> : StructBase where T : struct {
        public T value { get; set; }
        private T default_value;
        public GameText title { get; private set; }
        public GameText desc { get; private set; }
        public Settings.Category category { get; private set; }

        public Setting(T v, Settings.Category c, GameText t, GameText d) {
            value = v;
            category = c;
            default_value = v;
            title = t;
            desc = d == null ? t : d;
        }

        public Setting(SettingRuleElement<T> rule) {
            value = rule.default_value;
            default_value = rule.default_value;
            category = rule.category;
            title = rule.title;
            desc = rule.description == null ? rule.title : rule.description;
        }

        public void setDefault() {
            value = default_value;
        }
    }
}