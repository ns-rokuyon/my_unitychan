using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MenuAbilityButtonGroup : MonoBehaviour {
        public Ability.GroupId group_id;

        public List<MenuAbilityButton> buttons { get; protected set; }
        public ScrollRect scroll_rect { get; protected set; }

        public List<Button> ui_buttons {
            get {
                return buttons.Select(b => b.button).ToList();
            }
        }

        void Awake() {
            scroll_rect = GetComponent<ScrollRect>();

            buttons = new List<MenuAbilityButton>();
            buttons = scroll_rect.content.GetComponentsInChildren<MenuAbilityButton>().ToList();
        }

        void Start() {
            for ( int i = 0; i < ui_buttons.Count; i++ ) {
                var next = i + 1 >= ui_buttons.Count ? ui_buttons[0] : ui_buttons[i + 1];
                var prev = i - 1 < 0 ? ui_buttons[ui_buttons.Count - 1] : ui_buttons[i - 1];
                var navigation = ui_buttons[i].navigation;

                navigation.selectOnLeft = prev;
                navigation.selectOnRight = next;

                ui_buttons[i].navigation = navigation;
            }
        }
    }
}