using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class MissilePod : TurretBase {

        [SerializeField]
        public Const.ID.Projectile.Missile missile_id;

        private Missile prefab;
        private ReactiveProperty<int> stock = new ReactiveProperty<int>(0);
        private ICharacterMissileTankOwnable tank_owner;

        public int Stock {
            get { return stock.Value; }
        }

        public int StockMax {
            get { return StockMaxStream.Value; }
        }

        public ReadOnlyReactiveProperty<int> StockStream {
            get { return stock.ToReadOnlyReactiveProperty(); }
        }

        public ReadOnlyReactiveProperty<int> StockMaxStream {
            get { return tank_owner.MissileTankNumStream.Select(n => Const.Unit.ADDITIONAL_MISSILES * n).ToReadOnlyReactiveProperty(); }
        }

        public ProjectileSpec spec {
            get { return prefab.spec; }
        }

        void Awake() {
            tank_owner = GetComponent<ICharacterMissileTankOwnable>();
        }

        public override void Start() {
            base.Start();

            setMissile();
            stock.Value = StockMax;

            this.ObserveEveryValueChanged(_ => missile_id)
                .Subscribe(id => setMissile())
                .AddTo(this);
        }

        public override void shoot() {
            if ( Stock < prefab.cost )
                return;

            stock.Value -= prefab.cost;

            Missile missile = ProjectileManager.createMissile<Missile>(missile_id);
            missile.setDir(direction);
            missile.linkShooter(this);
            missile.fire(transform.position + muzzle_offset, owner.isLookAhead() ? 1.0f : -1.0f);

            // hitbox
            ProjectileHitbox hitbox = missile.GetComponentInChildren<ProjectileHitbox>();
            hitbox.depend_on_parent_object = true;
            hitbox.setOwner(gameObject);
            hitbox.setEnabledCollider(true);
            hitbox.ready(missile.gameObject, missile.spec);

            // sound
            sound();
        }

        public void setMissile() {
            var obj = ConfigTableManager.Missile.getPrefabConfig(missile_id).prefab;
            prefab = obj.GetComponent<Missile>();

            n_round_burst = spec.n_round_burst;
            burst_delta_frame = spec.burst_delta_frame;
            interval_frame = spec.interval_frame;
            se_id = spec.se_id;
            hitbox_id = spec.hitbox_id;
        }

        public void setMissile(Const.ID.Projectile.Missile id) {
            missile_id = id;
            setMissile();
        }

        public void supplyAmmo(int n) {
            stock.Value += n;
            if ( Stock > StockMax ) {
                stock.Value = StockMax;
            }
        }

    }
}
