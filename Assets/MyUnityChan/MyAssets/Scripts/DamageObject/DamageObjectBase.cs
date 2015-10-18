using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class DamageObjectBase : PoolObjectBase {

        void Awake() {
            commonSetting();
        }

        public void commonSetting() {
            int ch_layer = LayerMask.NameToLayer("Character");
            int do_layer = LayerMask.NameToLayer("DamageObject");

            Physics.IgnoreLayerCollision(ch_layer, do_layer);
        }
    }
}
