using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GateDoor : Door {
        public override void open() {
            transform.localScale = new Vector3(0.0f, transform.localScale.y, transform.localScale.z);
            GetComponent<Collider>().enabled = false;
        }

        public override void close() {
            transform.localScale = new Vector3(2.2f, transform.localScale.y, transform.localScale.z);
            GetComponent<Collider>().enabled = true;
        }
    }
}
