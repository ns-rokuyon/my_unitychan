using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace MyUnityChan {
    [RequireComponent(typeof(SoundPlayer))]
    public class MenuAbilityButton : GUIObjectBase, ISelectHandler, IDeselectHandler {
        [SerializeField]
        public Ability.Id ability_id;

        private RectTransform rect_transform;
        private Text description;

        public PlayerAbility ability { get; set; } 
        public Button button { get; protected set; }
        public RawImage icon { get; protected set; }
        public PlayerDemo demo {
            get {
                return MenuManager.Instance.player_demo;
            }
        }

        private Color32 icon_default_color;

        void Awake() {
            setupSoundPlayer();
            rect_transform = GetComponent<RectTransform>();
            description = transform.parent.GetComponentInChildren<Text>();
            button = GetComponent<Button>();
            icon = GetComponentInChildren<RawImage>();
            if ( icon )
                icon_default_color = icon.color;
        }

        void Start() {
            button.onClick.AddListener(press);

            ability = GameStateManager.self().player_manager.status.getAbility(ability_id);
            this.ObserveEveryValueChanged(_ => ability.status)
                .Subscribe(s => {
                    if ( s == Ability.Status.NO_GET ) {
                        icon.enabled = false;
                    }
                    else if ( s == Ability.Status.OFF ) {
                        icon.color = new Color32(icon_default_color.r,
                                                 icon_default_color.g,
                                                 icon_default_color.b,
                                                 16);
                    }
                    else if ( s == Ability.Status.ON ) {
                        icon.enabled = true;
                        icon.color = icon_default_color;
                    }
                });
        }

        public void OnSelect(BaseEventData eventData) {
            se(Const.ID.SE.BUTTON_SELECT);
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, -10.0f);
            demo.centering();
            demo.setDemoCameraDistance(ability.def.demo_camera_distance);
            ability.def.onSelectAbilityButton(demo.pm);
            if ( ability.status == Ability.Status.NO_GET ) {
                description.text = "?????";
                demo.play(ability.def.demo);
            }
            else {
                description.text = ability.def.name.get();
                demo.play(ability.def.demo);
            }
        }

        public void OnDeselect(BaseEventData eventData) {
            rect_transform.localPosition = rect_transform.localPosition.add(0, 0, 10.0f);
            ability.def.onDeSelectAbilityButton(demo.pm);
            demo.centering();
        }

        public void press() {
            GameStateManager.self().player_manager.status.toggleAbilityStatus(ability_id);
        }
    }

}
