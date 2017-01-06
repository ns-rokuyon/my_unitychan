using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace MyUnityChan {
    public class MenuAbilityButton : GUIObjectBase, ISelectHandler, IDeselectHandler {
        [SerializeField]
        public Ability.Id ability_id;

        private RectTransform rect_transform;
        private Text description;

        public PlayerAbility ability { get; set; } 

        void Awake() {
            setupSoundPlayer();
            rect_transform = GetComponent<RectTransform>();
            description = transform.parent.GetComponentInChildren<Text>();
        }

        void Start() {
            ability = GameStateManager.self().player_manager.status.getAbility(ability_id);
            this.ObserveEveryValueChanged(_ => ability.status)
                .Subscribe(s => {
                    if ( s == Ability.Status.NO_GET ) {
                        GetComponent<RawImage>().enabled = false;
                    }
                    else if ( s == Ability.Status.OFF ) {
                    }
                    else if ( s == Ability.Status.ON ) {
                        GetComponent<RawImage>().enabled = true;
                    }
                });
        }

        public void OnSelect(BaseEventData eventData) {
            se(Const.ID.SE.BUTTON_SELECT);
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, -10.0f);
            if ( ability.status == Ability.Status.NO_GET ) {
                description.text = "?????";
            }
            else {
                description.text = ability.def.name.get();
            }
        }

        public void OnDeselect(BaseEventData eventData) {
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, 10.0f);
        }

        public void press() {
            GameStateManager.self().player_manager.status.toggleAbilityStatus(ability_id);
        }
    }

}
