using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Attractable : ObjectBase {
        public Transform target;
        public float speed = 10.0f;

        private bool has_rigidbody { get; set; }
        private System.IDisposable updater { get; set; }

        void Start() {
            has_rigidbody = rigid_body.rb != null;

            if ( has_rigidbody ) {
                throw new System.NotImplementedException();
            }
        }

        public void go() {
            if ( !target )
                return;

            if ( updater != null )
                return;

            if ( time_control == null ) {
                DebugManager.log(name + ": TimeControllable component is not attached");
                return;
            }
            updater = time_control.PausableEveryUpdate()
                .Subscribe(_ => {
                    transform.position = Vector3.MoveTowards(
                        transform.position, target.transform.position,
                        time_control.deltaTime * speed);
                })
                .AddTo(this);
        }

        public void clear() {
            if ( updater == null )
                return;
            updater.Dispose();
            updater = null;
        }
    }
}