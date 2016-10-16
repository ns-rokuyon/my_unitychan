using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using EnergyBarToolkit;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class HpGauge : GUIObjectBase {
        protected EnergyBar energybar;
        protected FilledRendererUGUI renderer;
        protected Slider slider;
        protected Character character;

        public bool auto_hidden;

        void Awake() {
            energybar = GetComponent<EnergyBar>();
            renderer = GetComponent<FilledRendererUGUI>();
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
            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
