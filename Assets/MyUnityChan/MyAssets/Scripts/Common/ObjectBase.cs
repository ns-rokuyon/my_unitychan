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

        public void adjustZtoBaseline() {
            Area area = AreaManager.Instance.getAreaFromMemberObject(this.gameObject);
            if ( !area.isEmptyBaselineZ() ) {
                float z = area.getBaselineZ();
                this.gameObject.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y,
                    z
                    );
            }
        }

        protected void setupSoundPlayer() {
            sound = gameObject.GetComponent<SoundPlayer>();
        }

        public SoundPlayer getSoundPlayer() {
            return sound;
        }

        public void OnTriggerEnter(Collider other) {
            Debug.Log("OnEnter");
            if ( other.tag == "MovingFloor" && transform.parent == null ) {
                transform.parent = other.gameObject.transform;
                other.gameObject.GetComponent<MovingFloor>().getOn(this);
            }
        }

        public void OnTriggerExit(Collider other) {
            if ( other.tag == "MovingFloor" && transform.parent != null ) {
                transform.parent = null;
                other.gameObject.GetComponent<MovingFloor>().getOff(this);
            }
        }

    }
}
