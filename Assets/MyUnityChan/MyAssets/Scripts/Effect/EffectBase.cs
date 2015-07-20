using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class EffectBase : PoolObjectBase {
        protected string resource_path = null;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

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