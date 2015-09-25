using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Timer : PoolObjectBase {
        protected bool use_objectpool;
        protected bool running = false;
        protected string resource_path = null;

        public bool isRunning() {
            return running;
        }

        public bool finished() {
            return !running;
        }

        public void destroy() {
            if ( use_objectpool ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, resource_path);
            }
            else {
                Destroy(this.gameObject);
            }
        }

        public virtual void enablePool(string _resource_path) {
            resource_path = _resource_path;
            use_objectpool = true;
        }
    }
}