using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using EnergyBarToolkit;
using DG.Tweening;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class HpGauge : GUIObjectBase {
        public EnergyBar energybar { get; protected set; }
        public Character character { get; protected set; }
        public FilledRendererUGUI renderer { get; protected set; }
        public RectTransform rect_transform { get; protected set; }

        public bool auto_hidden;

        void Awake() {
            renderer = GetComponent<FilledRendererUGUI>();
            energybar = GetComponent<EnergyBar>();
            rect_transform = GetComponent<RectTransform>();
            character = null;
        }

        void Start() {
            // Sync HP
            this.UpdateAsObservable()
                .Where(_ => character != null && energybar != null)
                .Subscribe(_ => energybar.SetValueCurrent(character.getHP()));

            // Decrement alpha in bar color
            this.UpdateAsObservable()
                .Where(_ => auto_hidden && renderer != null)
                .Where(_ => renderer.spriteBarColor.a > 0.0f)
                .Subscribe(_ => {
                    renderer.spriteBarColor.a -= Const.Unit.HP_BAR_ALPHA_DECREASE_SPEED;
                    foreach ( var s in renderer.spritesForeground ) {
                        s.color.a -= Const.Unit.HP_BAR_ALPHA_DECREASE_SPEED;
                    }
                });

            // Reset alpha to 1.0
            this.ObserveEveryValueChanged(_ => energybar.valueCurrent)
                .Where(_ => auto_hidden && renderer != null)
                .Subscribe(_ => {
                    renderer.spriteBarColor.a = 1.0f;
                    foreach ( var s in renderer.spritesForeground ) {
                        s.color.a = 1.0f;
                    }
                });
        }

        public virtual void setCharacter(Character ch) {
            character = ch;
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
