using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Invoker : ObjectBase {
        protected abstract void invoke();
        protected abstract bool schedule();

        protected virtual void close() {
            Destroy(this.gameObject);
        }

        void Update() {
            if ( schedule() ) {
                invoke();
                close();
            }
        }
    }
}
