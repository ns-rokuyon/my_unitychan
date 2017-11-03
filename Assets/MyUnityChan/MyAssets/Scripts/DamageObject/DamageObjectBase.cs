using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class DamageObjectBase : PoolObjectBase {

        public bool has_hitbox_in_children;

        public virtual void Awake() {
            commonSetting();
        }

        public virtual void setStartPosition(Vector3 pos) {
            transform.position = pos;
        }

        public void commonSetting() {
            int ch_layer = LayerMask.NameToLayer("Character");
            int do_layer = LayerMask.NameToLayer("DamageObject");

            Physics.IgnoreLayerCollision(ch_layer, do_layer);
        }
    }
}
