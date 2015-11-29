using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FlameThrower : LauncherBase {

        public float power;
        public Vector3 base_angle = new Vector3(0.7f, 1.0f, 0.0f);
        public string flame_name;

        void Start() {
            baseStart();
            setProjectile(flame_name);
        }

        public override Vector3 angle() {
            if ( this.gameObject.transform.forward.x >= 0 ) return base_angle;
            return base_angle.flipX();
        }


        public override void shoot() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[flame_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);
            obj.GetComponent<Rigidbody>().AddForce(angle() * power, ForceMode.Impulse);

            Flame flame = obj.GetComponent<Flame>();
            flame.setDir(angle());
            flame.setStartPosition(this.gameObject.transform.position);

            // hitbox
            DamageObjectHitbox hitbox = HitboxManager.self().create<DamageObjectHitbox>(Const.Prefab.Hitbox[hitbox_name], use_objectpool:true);
            hitbox.setOwner(this.gameObject);
            hitbox.ready(obj, flame.spec);

            // sound
            sound();

            /*
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile["FLAME"]);
            obj.GetComponent<Flame>().getHitbox().setOwner(this.gameObject);
            obj.setParent(Hierarchy.Layout.DAMAGE_OBJECT);
            obj.transform.position = this.gameObject.transform.position;

            // hitbox
            ProjectileHitbox hitbox = HitboxManager.self().create<ProjectileHitbox>(Const.Prefab.Hitbox[hitbox_name], use_objectpool:true);
            hitbox.setOwner(this.gameObject);
            hitbox.ready(obj, beam.spec);
            */
        }
    }
}
