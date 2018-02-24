using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public abstract class TurretBase : ShooterBase {
        public override void Start() {
            base.Start();

            this.UpdateAsObservable()
                .Where(_ => owner != null)
                .Where(_ => !shooting)
                .Where(_ => !owner.time_control.paused && !owner.isFrozen() && !owner.isFlinching())
                .Where(_ => auto || triggered)
                .Subscribe(_ => StartCoroutine(shootByTrigger()));
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
