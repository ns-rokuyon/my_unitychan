using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GateDoor : Door {
        void Awake() {
            setupSoundPlayer();
        }

        public override void open() {
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            se(Const.ID.SE.GATE_OPEN);
        }

        public override void close() {
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
