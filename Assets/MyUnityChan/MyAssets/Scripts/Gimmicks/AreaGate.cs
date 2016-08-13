using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AreaGate : ObjectBase {
        public GameObject gate_pair;

        public bool pass { get; set; }

        void Awake() {
            pass = false;

            GameObject dst = gate_pair.transform.FindChild("GateDestination").gameObject;

            // set destination to gate start collision
            var warp = transform.FindChild("GateStart").gameObject.GetComponent<Warp>();
            warp.warp_to = dst;
            (warp as AreaGateWarpCollision).gate = this;
        }

        void Start() {
            AreaManager.self().registerAreaConnectionInfo(this.gameObject, gate_pair);
        }

        public void onPass() {
            pass = true;
            gate_pair.GetComponent<AreaGate>().pass = true;
        }
    }
}
