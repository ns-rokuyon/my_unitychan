using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MenuAbilityButtonGroup : GUIObjectBase {
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
            UIHelper.makeExplicitNavigation(ui_buttons, UIHelper.LayoutDirection.HORIZONTAL);
        }
    }
}