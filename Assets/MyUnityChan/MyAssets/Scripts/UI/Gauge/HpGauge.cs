using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using EnergyBarToolkit;
using DG.Tweening;
using TMPro;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class HpGauge : GUIObjectBase {
        public EnergyBar energybar { get; protected set; }
        public Character character { get; protected set; }
        public FilledRendererUGUI renderer { get; protected set; }
        public RectTransform rect_transform { get; protected set; }
        public TextMeshProUGUI sync_label { get; protected set; }
        public IDisposable follower { get; protected set; }

        public bool auto_hidden;

        void Awake() {
            renderer = GetComponent<FilledRendererUGUI>();
            energybar = GetComponent<EnergyBar>();
            rect_transform = GetComponent<RectTransform>();
            sync_label = GetComponentInChildren<TextMeshProUGUI>();
            character = null;
        }

        void Start() {
            // Sync HP
            this.UpdateAsObservable()
                .Where(_ => character != null && energybar != null)
                .Subscribe(_ => {
                    energybar.SetValueCurrent(character.getHP());
                    if ( sync_label )
                        sync_label.text = character.getHP().ToString();
                });

            // Decrement alpha in bar color
            this.UpdateAsObservable()
                .Where(_ => auto_hidden && !PauseManager.isPausing() && renderer != null)
                .Where(_ => renderer.spriteBarColor.a > 0.0f)
                .Subscribe(_ => {
                    renderer.spriteBarColor.a -= Const.Unit.HP_BAR_ALPHA_DECREASE_SPEED;
                    foreach ( var s in renderer.spritesForeground ) {
                        s.color.a -= Const.Unit.HP_BAR_ALPHA_DECREASE_SPEED;
                    }
                    foreach ( var s in renderer.spritesBackground ) {
                        s.color.a -= Const.Unit.HP_BAR_ALPHA_DECREASE_SPEED;
                    }
                });

            // Reset alpha to 1.0
            this.ObserveEveryValueChanged(_ => energybar.valueCurrent)
                .Where(_ => auto_hidden && !PauseManager.isPausing() && renderer != null)
                .Subscribe(_ => {
                    renderer.spriteBarColor.a = 1.0f;
                    foreach ( var s in renderer.spritesForeground ) {
                        s.color.a = 1.0f;
                    }
                    foreach ( var s in renderer.spritesBackground ) {
                        s.color.a = 1.0f;
                    }
                });
        }

        public virtual void setCharacter(Character ch) {
            character = ch;
        }

        public void follow(Character ch) {
            unfollow();
            setCharacter(ch);

            follower = this.UpdateAsObservable()
                .Where(_ => character != null && character.gameObject.activeSelf)
                .Subscribe(_ => {
                    transform.position = character.transform.position + character.worldspace_ui_position_offset;
                });
        }

        public void unfollow() {
            if ( follower == null )
                return;
            follower.Dispose();
            follower = null;
        }

        public void setMapHp(int maxhp) {
            energybar.SetValueMax(maxhp);
        }

        public void setPosition(Vector3 pos) {
            rect_transform.anchoredPosition = pos;
        }

        public void shake() {
            rect_transform.DOShakePosition(0.5f);
        }
    }
}
