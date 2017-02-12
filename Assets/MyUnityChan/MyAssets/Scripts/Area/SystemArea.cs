using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SystemArea : Area {
        public override void OnTriggerEnter(Collider colliderInfo) {
            DebugManager.log(colliderInfo.gameObject.name);
        }
    }
}