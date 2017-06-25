using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class PlayerAbilityManager : SingletonObjectBase<PlayerAbilityManager> {

        [Tooltip("The reference to Text component to display the ability description of current selected ability button")]
        public Text description;

        [Tooltip("References to ScrollRect component which has MenuAbilityButtonGroup components in children")]
        public ScrollRect ability_groups_parent;

        public Dictionary<Ability.GroupId, MenuAbilityButtonGroup> ability_button_groups;

        void Awake() {
            ability_button_groups = new Dictionary<Ability.GroupId, MenuAbilityButtonGroup>();
            ability_groups_parent.content.GetComponentsInChildren<MenuAbilityButtonGroup>().ToList().ForEach(g => {
                ability_button_groups.Add(g.group_id, g);
            });
        }

        void Start() {
            // Setup explicit navigations for ability buttons
            int ngroups = ability_button_groups.Keys.Count;
            for ( int i = 0; i < ngroups; i++ ) {
                Ability.GroupId gid = (Ability.GroupId)i;
                var next_row = i + 1 >= ngroups ? ability_button_groups[0] : ability_button_groups[gid + 1];
                var prev_row = i - 1 < 0 ? ability_button_groups[(Ability.GroupId)ngroups - 1] : ability_button_groups[gid - 1];
                var current_row = ability_button_groups[gid];
                for ( int j = 0; j < current_row.ui_buttons.Count; j++ ) {
                    var b = current_row.ui_buttons[j];
                    var navigation = b.navigation;
                    var next_cell = j < next_row.ui_buttons.Count ? next_row.ui_buttons[j] : next_row.ui_buttons[0];
                    var prev_cell = j < prev_row.ui_buttons.Count ? prev_row.ui_buttons[j] : prev_row.ui_buttons[0];
                    // Vertical cursor move
                    navigation.selectOnUp = prev_cell;
                    navigation.selectOnDown = next_cell;
                    b.navigation = navigation;
                }
            }
        }

        public static void updateDescription(string desc) {
            Instance.description.text = desc;
        }
    }
}