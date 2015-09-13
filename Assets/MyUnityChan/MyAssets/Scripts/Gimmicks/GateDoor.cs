using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GateDoor : Door {
        public override void open() {
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }

        public override void close() {
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
