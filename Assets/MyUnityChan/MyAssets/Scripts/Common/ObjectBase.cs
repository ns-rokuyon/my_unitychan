using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ObjectBase : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTriggerEnter(Collider other) {
            if ( other.tag == "MovingFloor" && transform.parent == null ) {
                transform.parent = other.gameObject.transform;
            }
        }

        public void OnTriggerExit(Collider other) {
            if ( other.tag == "MovingFloor" && transform.parent != null ) {
                transform.parent = null;
            }
        }

    }
}
