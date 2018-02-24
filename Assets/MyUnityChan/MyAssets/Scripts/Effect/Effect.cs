using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Effect : EffectBase {
        public int total_frame { get; private set; }

        public void ready(Vector3 pos, int frame) {
            total_frame = frame;
            transform.position = pos;

            if ( !managed_by_objectpool ) {
                // Call initialize() unless non pool object
                initialize();
            }

            onReady();

            if ( frame >= 0 ) {
                // Timer to destroy itself
                time_control.PausableTimerFrame(frame)
                    .Subscribe(_ => destroy())
                    .AddTo(gameObject);
            }
        }

        public void destroy() {
            if ( !gameObject )
                return;

            if ( pooled )
                ObjectPoolManager.releaseGameObject(this);
            else
                Destroy(gameObject);
        }

        public override void initialize() {
            base.initialize();
            setupSoundPlayer();
        }

        public override void finalize() {
            base.finalize();
        }

        protected virtual void onReady() {
        }
    }
}