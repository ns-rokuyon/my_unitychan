using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FlameThrower : LauncherBase {

        public bool on = false;
        public float power = 1.0f;
        public Vector3 direction = new Vector3(0.7f, 1.0f, 0.0f);


        // Use this for initialization
        void Start() {
            sleep = false;
            frame_count = 0;
        }

        // Update is called once per frame
        void Update() {
            if ( !on ) return;

            if ( sleep ) {
                if ( Time.frameCount - sleep_start_frame >= interval_frame ) {
                    sleep = false;
                    return;
                }
                return;
            }

            setDirX(this.gameObject.transform.forward);

            if ( frame_count > 0 ) {
                if ( shooting_frame <= frame_count ) {
                    frame_count = 0;
                    sleep = true;
                    sleep_start_frame = Time.frameCount;
                    return;
                }
                shoot();
                frame_count++;
            }
            else {
                shoot();
                frame_count = 1;
            }
        }

        public void setDirX(Vector3 dir) {
            if ( dir.x >= 0 ) {
                direction.x = Mathf.Abs(direction.x);
            }
            else {
                direction.x = -Mathf.Abs(direction.x);
            }
        }

        public void shoot() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Phenomenon["FLAME"]);
            obj.GetComponent<Flame>().getHitbox().setOwner(this.gameObject);
            obj.setParent(Hierarchy.Layout.DAMAGE_OBJECT);
            obj.transform.position = this.gameObject.transform.position;
            obj.GetComponent<Rigidbody>().AddForce(direction * 10.0f, ForceMode.Impulse);
        }
    }
}
