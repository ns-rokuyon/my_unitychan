using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MissilePod : TurretBase {
        public string missile_name;

        void Start() {
            baseStart();
            setProjectile(missile_name);
        }

        public override void shoot() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[missile_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Missile missile = obj.GetComponent<Missile>();
            missile.setDir(angle());
            missile.fire(transform.position, owner.isLookAhead() ? 1.0f : -1.0f);

            // hitbox
            ProjectileHitbox hitbox = missile.GetComponentInChildren<ProjectileHitbox>();
            hitbox.setOwner(gameObject);
            hitbox.setEnabledCollider(true);
            hitbox.ready(obj, missile.spec);

            // sound
            sound();
        }


    }
}
