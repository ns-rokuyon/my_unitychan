using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AreaGate : ObjectBase {
        public GameObject gate_pair;

        void Awake() {
            GameObject dst = gate_pair.transform.FindChild("GateDestination").gameObject;

            // set destination to gate start collision
            transform.FindChild("GateStart").gameObject.GetComponent<Warp>().warp_to = dst;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
