using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using EnergyBarToolkit;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class ProgressGauge : GUIObjectBase {
        public GameObject caption_object;
        public GameText caption;
        [SerializeField]
        public Const.ID.Progress target;

        protected EnergyBar bar;
        protected Text caption_text;

        void Awake() {
            bar = GetComponent<EnergyBar>();
            if ( caption_object )
                caption_text = caption_object.GetComponent<Text>();
        }

        void Start() {
            Observable.EveryUpdate()
                .Where(_ => GameStateManager.now() == GameStateManager.GameState.MENU)
                .Subscribe(_ => bar.valueMax = getMax());

            Observable.EveryUpdate()
                .Where(_ => GameStateManager.now() == GameStateManager.GameState.MENU)
                .Subscribe(_ => bar.valueCurrent = getCurrent());

            Observable.EveryUpdate()
                .Where(_ => GameStateManager.now() == GameStateManager.GameState.MENU)
                .Subscribe(_ => caption_text.text = caption.get());
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
