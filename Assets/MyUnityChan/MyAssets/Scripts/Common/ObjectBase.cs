using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ObjectBase : MonoBehaviour {

        protected SoundPlayer sound;

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

        public virtual bool assert() {
            return true;
        }

        protected void setupSoundPlayer() {
            sound = gameObject.GetComponent<SoundPlayer>();
        }

        public SoundPlayer getSoundPlayer() {
            return sound;
        }

        public void se(Const.ID.SE sid, bool playOneShot=true, int delay = 0) {
            if ( sound == null ) {
                DebugManager.log(name + " doesn't have SoundPlayer component", Const.Loglevel.WARN);
                return;
            }
            sound.play(sid, playOneShot, delay);
        }

        public string prefabPath(Const.ID.Controller name) {
            return Const.Prefab.Controller[name];
        }

        public string prefabPath(Const.ID.Effect name) {
            return Const.Prefab.Effect[name];
        }

        public string prefabPath(Const.ID.Item name) {
            return Const.Prefab.Item[name];
        }

        public void OnTriggerEnter(Collider other) {
            if ( other.tag == "MovingFloor" ) {
                other.gameObject.GetComponent<MovingFloor>().getOn(this);
            }
        }

        public void OnTriggerExit(Collider other) {
            if ( other.tag == "MovingFloor" ) {
                other.gameObject.GetComponent<MovingFloor>().getOff(this);
            }
        }

    }
}
