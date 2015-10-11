using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ObjectBase : MonoBehaviour {

        protected SoundPlayer sound;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        protected void setupSoundPlayer() {
            sound = gameObject.GetComponent<SoundPlayer>();
        }

        public SoundPlayer getSoundPlayer() {
            return sound;
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
