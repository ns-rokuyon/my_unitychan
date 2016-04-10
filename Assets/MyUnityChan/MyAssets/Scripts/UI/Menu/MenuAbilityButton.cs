using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace MyUnityChan {
    public class MenuAbilityButton : GUIObjectBase, ISelectHandler, IDeselectHandler {
        [SerializeField]
        public Ability.Id ability_id;

        private RectTransform rect_transform;

        void Awake() {
            setupSoundPlayer();
            rect_transform = GetComponent<RectTransform>();
        }

        public void OnSelect(BaseEventData eventData) {
            sound.play(Const.Sound.SE.UI["BUTTON_SELECT"], true);
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, -10.0f);
        }

        public void OnDeselect(BaseEventData eventData) {
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, 10.0f);
        }

        public void press() {
            GameStateManager.self().player_manager.status.toggleAbilityStatus(ability_id);
        }
    }

}
