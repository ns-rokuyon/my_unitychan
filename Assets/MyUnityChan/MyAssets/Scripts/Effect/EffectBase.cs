using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(SoundPlayer))]
    public abstract class EffectBase : PoolObjectBase {
        protected string resource_path = null;

        public override void initialize() {
            throw new System.NotImplementedException();
        }

        public override void finalize() {
            throw new System.NotImplementedException();
        }

        public virtual void enablePool(string _resource_path) {
            resource_path = _resource_path;
        }
    }
}