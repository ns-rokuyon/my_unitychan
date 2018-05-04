using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using System;


namespace MyUnityChan {
    public class Bomber : ObjectBase {

        [SerializeField]
        public Const.ID.Bomb bomb_id;

        private IDisposable reloader;
        private Bomb prefab;
        private List<ICommunicatableBomb> communicatable_bombs = new List<ICommunicatableBomb>();
        private ReactiveProperty<int> stock = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> stock_max = new ReactiveProperty<int>(0);

        public int Stock {
            get { return stock.Value; }
        }

        public int StockMax {
            get { return stock_max.Value; }
        }

        public ReadOnlyReactiveProperty<int> StockStream {
            get { return stock.ToReadOnlyReactiveProperty(); }
        }

        public ReadOnlyReactiveProperty<int> StockMaxStream {
            get { return stock_max.ToReadOnlyReactiveProperty(); }
        }

        public BombSpec spec {
            get { return prefab.spec; }
        }

        void Start() {
            setBomb();

            this.ObserveEveryValueChanged(_ => bomb_id)
                .Subscribe(id => setBomb())
                .AddTo(this);
        }

        // Put a new bomb at the given position
        public Bomb put(Vector3 pos) {
            if ( stock.Value == 0 )
                return null;

            Bomb bomb = DamageObjectManager.createBomb(bomb_id);
            bomb.setStartPosition(pos);
            stock.Value--;

            if ( bomb is ICommunicatableBomb ) {
                communicatable_bombs.Add(bomb as ICommunicatableBomb);
            }
            if ( bomb is IGUIWorldSpaceUILinked ) {
                (bomb as IGUIWorldSpaceUILinked).createWorldUI(pos);
            }
            return bomb;
        }

        // Communicate with managed bomb
        public bool communicate() {
            List<ICommunicatableBomb> accepts = new List<ICommunicatableBomb>();

            communicatable_bombs.ForEach(cb => {
                bool accepted = cb.communicate(this);
                if ( accepted )
                    accepts.Add(cb);
            });

            if ( accepts.Count == 0 )
                return false;

            accepts.ForEach(cb => communicatable_bombs.Remove(cb));
            return true;
        }

        // Get initial position to put
        public Vector3 getInitPosition(Transform owner) {
            return prefab.getInitPosition(owner);
        }

        public void setBomb() {
            var obj = ConfigTableManager.Bomb.getPrefabConfig(bomb_id).prefab;
            prefab = obj.GetComponent<Bomb>();
            stock.Value = spec.stock_max;
            stock_max.Value = spec.stock_max;
            setAutoReloader();
        }

        public void setBomb(Const.ID.Bomb id) {
            bomb_id = id;
            setBomb();
        }

        public void reloadFull() {
            stock.Value = StockMax;
        }

        private void setAutoReloader() {
            if ( reloader != null ) {
                reloader.Dispose();
                reloader = null;
            }

            if ( !spec.auto_reload )
                return;

            reloader = time_control.PausableIntervalFrame(spec.reload_frame)
                .Subscribe(_ => {
                    if ( stock.Value < spec.stock_max ) stock.Value++;
                }).
                AddTo(this);
        }
    }
}