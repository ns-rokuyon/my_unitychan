using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public abstract class TurretBase : ShooterBase {
        protected string projectile_name;

        public override void Start() {
            base.Start();

            this.UpdateAsObservable()
                .Where(_ => owner != null)
                .Where(_ => !shooting)
                .Where(_ => !owner.time_control.paused && !owner.isFrozen() && !owner.isFlinching())
                .Where(_ => auto || triggered)
                .Subscribe(_ => StartCoroutine(shootByTrigger()));
        }

        protected void setProjectile(string name) {
            projectile_name = name;
            Projectile proj = (Resources.Load(Const.Prefab.Projectile[projectile_name]) as GameObject).GetComponent<Projectile>();
            ProjectileSpec spec = proj.spec;

            n_round_burst = spec.n_round_burst;
            burst_delta_frame = spec.burst_delta_frame;
            interval_frame = spec.interval_frame;
            se_name = spec.se_name;
            hitbox_name = spec.hitbox_name;
        }

        protected IEnumerator shootByTrigger() {
            shooting = true;
            if ( n_round_burst == 1 ) {
                shoot();
            }
            else {
                for ( int i = 0; i < n_round_burst; i++ ) {
                    shoot();
                    int delta = burst_delta_frame;
                    while ( delta > 0 ) {
                        delta--;
                        yield return null;
                    }
                }
            }
            int interval = interval_frame;
            while ( interval > 0 ) {
                interval--;
                yield return null;
            }
            shooting = false;
        }
    }
}
