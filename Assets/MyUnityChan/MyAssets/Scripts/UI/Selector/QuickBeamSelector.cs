using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

namespace MyUnityChan {
    public class QuickBeamSelector : QuickSelector {
        public MenuAbilityButtonGroup abs { get; protected set; }
        public Dictionary<Button, BeamAbility> button_ability_map { get; protected set; }

        public override string button_prefab_path {
            get {
                return "Prefabs/UI/Selector/QuickSelectorButton";
            }
        }

        protected override void init() {
            abs = PlayerAbilityManager.self().ability_button_groups[Ability.GroupId.BEAM];
            button_ability_map = new Dictionary<Button, BeamAbility>();
        }

        protected override void setupButtons() {
            Player player = GameStateManager.getPlayer().manager.getPlayer(Const.CharacterName.UNITYCHAN);
            player.beam_slot.ForEach(beamname => {
                MenuAbilityButton b = getBeamAbilityButtonByBeamName(beamname);
                BeamAbility ability = (b.ability.def as BeamAbility);
                if ( button_ability_map.ContainsValue(ability) ) {
                    // Displayed button
                    if ( selected_button )
                        selected_button.GetComponentInChildren<Text>().text = ability.name.get();
                    return;
                }
                // Instantiate a button
                Button button = PrefabInstantiater.createUIAndGetComponent<Button>(button_prefab_path, content);
                EventTrigger trigger = button.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entry.callback.AddListener((data) => onSelect(data));
                trigger.triggers.Add(entry);
                RawImage image = button.GetComponentInChildren<RawImage>();
                image.texture = b.GetComponentInChildren<RawImage>().texture;
                Text text = button.GetComponentInChildren<Text>();
                text.text = ability.name.get();

                button_ability_map.Add(button, ability);
            });
            UIHelper.makeExplicitNavigation(button_ability_map.Keys.ToList(),
                                            UIHelper.LayoutDirection.VERTICAL);
        }

        protected override void onClose() {
            button_ability_map.Keys.Where(b => b != selected_button).ToList().ForEach(b => {
                button_ability_map.Remove(b);
            });
            if ( selected_button )
                selected_button.GetComponentInChildren<Text>().text = "";
        }

        protected override Button getFirstSelected() {
            Player player = GameStateManager.getPlayer().manager.getPlayer(Const.CharacterName.UNITYCHAN);
            var ba = button_ability_map.Where(kv => kv.Value.beam_name == player.beam_turret.beam_name).FirstOrDefault();
            if ( ba.Equals(default(Dictionary<Button, BeamAbility>)) ) {
                return null;
            }
            return ba.Key;
        }

        public override void onSelect(BaseEventData data) {
            base.onSelect(data);

            if ( !selected_button ) {
                return;
            }
            // Switch to the beam on cursor
            Player player = GameStateManager.getPlayer().manager.getPlayer(Const.CharacterName.UNITYCHAN);
            player.beam_turret.switchBeam(button_ability_map[selected_button].beam_name);
        }

        private MenuAbilityButton getBeamAbilityButtonByBeamName(Const.BeamName bn) {
            MenuAbilityButton b = abs.buttons.Find(_b => {
                return (_b.ability.def as BeamAbility).beam_name == bn;
            });
            return b;
        }
    }
}