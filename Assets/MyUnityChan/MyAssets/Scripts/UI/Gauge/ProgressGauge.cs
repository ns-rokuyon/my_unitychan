using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using EnergyBarToolkit;
using DG.Tweening;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class ProgressGauge : GUIObjectBase {
        public GameObject caption_object;
        public GameText caption;
        [SerializeField]
        public Const.ID.Progress target;
        public bool bar_animation;

        protected EnergyBar bar;
        protected Text caption_text;

        public float progress {
            get {
                return bar.valueCurrent;    // percent
            }
            set {
                bar.valueCurrent = (int)value;   // percent
            }
        }

        protected float BAR_ANIMATION_SEC = 0.5f;

        void Awake() {
            bar = GetComponent<EnergyBar>();
            if ( caption_object )
                caption_text = caption_object.GetComponent<Text>();
        }

        void Start() {
            parent_canvas.ObserveEveryValueChanged(c => PauseManager.isPausing() && c.enabled)
                .Where(b => b)
                .Subscribe(_ => {
                    caption_text.text = caption.get();
                    bar.valueMax = 100;     // percent
                    if ( bar_animation ) {
                        progress = 0;
                        DOTween.To(() => progress, (x) => progress = x, getPercent(), BAR_ANIMATION_SEC)
                            .SetUpdate(true)
                            .SetEase(Ease.Linear);
                    }
                    else {
                        progress = getPercent();
                    }
                });
        }

        public float getPercent() {
            return getCurrent() / (float)getMax() * 100.0f;
        }

        public int getCurrent() {
            switch ( target ) {
                case Const.ID.Progress.ENERGY_TANK:
                    {
                        var status = GameStateManager.getPlayer().status as PlayerStatus;
                        return status.energy_tanks;
                    }
                case Const.ID.Progress.MISSILE_TANK:
                    {
                        var status = GameStateManager.getPlayer().status as PlayerStatus;
                        return status.missile_tanks;
                    }
                case Const.ID.Progress.AREA:
                    {
                        return AreaManager.getPassedAreaNum();
                    }
                default:
                    break;
            }
            return 0;
        }

        public int getMax() {
            switch ( target ) {
                case Const.ID.Progress.ENERGY_TANK:
                    {
                        return EnergyTank.count;
                    }
                case Const.ID.Progress.MISSILE_TANK:
                    {
                        return MissileTank.count;
                    }
                case Const.ID.Progress.AREA:
                    {
                        return AreaManager.getAreaNum();
                    }
                default:
                    break;
            }
            return 1;
        }
    }
}
