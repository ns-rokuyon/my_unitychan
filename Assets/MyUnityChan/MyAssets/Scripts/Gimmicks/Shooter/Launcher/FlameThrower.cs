using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FlameThrower : LauncherBase {

        public float power = 1.0f;
        public Vector3 base_angle = new Vector3(0.7f, 1.0f, 0.0f);

        public override Vector3 angle() {
            if ( this.gameObject.transform.forward.x >= 0 ) return base_angle;
            return base_angle.flipX();
        }


        public override void shoot() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.DamageObject["FLAME"]);
            obj.GetComponent<Flame>().getHitbox().setOwner(this.gameObject);
            obj.setParent(Hierarchy.Layout.DAMAGE_OBJECT);
            obj.transform.position = this.gameObject.transform.position;
            obj.GetComponent<Rigidbody>().AddForce(angle() * 10.0f, ForceMode.Impulse);
        }
    }
}
