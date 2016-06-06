using UnityEngine;
using System.Collections.Generic;
using System;

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

    public class SettingSelect : Setting<int> {
        public Type type { get; private set; }
        public List<GameText> item_texts { get; private set; }

        public SettingSelect(SettingSelectRuleElement rule) : 
            base(rule.default_value, rule.category, rule.title, rule.description) {
            type = rule.type;
            item_texts = new List<GameText>();
            foreach ( var it in rule.item_texts ) {
                item_texts.Add(it);
            }
        }

        public T selected<T>() {
            return (T)Enum.ToObject(type, value);
        }

        public void select(int v) {
            value = v;
        }
    }
}