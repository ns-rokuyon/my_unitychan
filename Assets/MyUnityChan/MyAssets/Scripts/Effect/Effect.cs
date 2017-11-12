using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Effect : EffectBase {
        public int total_frame { get; private set; }

        public void ready(Vector3 pos, int frame, string _resource_path) {
            total_frame = frame;
            resource_path = _resource_path;
            transform.position = pos;

            if ( !managed_by_objectpool ) {
                // Call initialize() unless non pool object
                initialize();
            }

            onReady();

            time_control.PausableTimerFrame(frame)
                .Subscribe(_ => destroy())
                .AddTo(gameObject);
        }

        public void destroy() {
            if ( !gameObject )
                return;

            if ( pooled )
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
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