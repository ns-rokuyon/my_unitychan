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

        private RectTransform rect_transform;           // The whole of button
        private RectTransform icon_rect_transform;      // Only icon image

        private Tweener scaler;
        private Sequence rotator;

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
            button = GetComponent<Button>();
            icon = GetComponentInChildren<RawImage>();
            icon_rect_transform = icon.GetComponent<RectTransform>();
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

            if ( scaler != null )
                scaler.Kill();
            // Icon size x1.5
            scaler = icon_rect_transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);

            if ( rotator != null ) {
                rotator.Complete();
                rotator.Kill();
            }
            // Icon rotation
            rotator = DOTween.Sequence();
            rotator.AppendInterval(3.0f);
            rotator.Append(icon_rect_transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.InOutCirc));
            rotator.SetLoops(-1);

            //demo.centering();
            //demo.setDemoCameraDistance(ability.def.demo_camera_distance);
            ability.def.onSelectAbilityButton(demo.pm);
            if ( ability.status == Ability.Status.NO_GET ) {
                PlayerAbilityManager.updateDescription("?????");
            }
            else {
                PlayerAbilityManager.updateDescription(ability.def.name.get());
                //demo.play(ability.def.demo);
            }
        }

        public void OnDeselect(BaseEventData eventData) {
            if ( scaler != null )
                scaler.Kill();
            // Fix icon size
            icon_rect_transform.DOScale(Vector3.one, 0.1f);

            if ( rotator != null ) {
                rotator.Complete();
                rotator.Kill();
            }
            // Fix icon to base angle
            icon_rect_transform.DORotate(new Vector3(0.0f, 0.0f, 0.0f), 1.0f, RotateMode.Fast);

            ability.def.onDeSelectAbilityButton(demo.pm);
            delay(1, () => demo.centering());
        }

        public void press() {
            GameStateManager.self().player_manager.status.toggleAbilityStatus(ability_id);
        }
    }

}
