using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Effect : EffectBase {
        public void ready(Vector3 pos, int frame, string _resource_path) {
            resource_path = _resource_path;
            transform.position = pos;

            initialize();

            Observable.TimerFrame(frame)
                .Subscribe(_ => destroy());
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
            setupSoundPlayer();
        }

        public override void finalize() {
        }
    }
}