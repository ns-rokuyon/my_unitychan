using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class BeamTurret : TurretBase {
        public string beam_name;

        void Start() {
            baseStart();
            setProjectile(beam_name);
        }

        public override void shoot() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[beam_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Beam beam = obj.GetComponent<Beam>();
            beam.setDir(angle());
            beam.setStartPosition(this.gameObject.transform.position);

            // hitbox
            ProjectileHitbox hitbox = HitboxManager.self().create<ProjectileHitbox>(Const.Prefab.Hitbox[hitbox_name], use_objectpool:true);
            hitbox.setOwner(this.gameObject);
            hitbox.ready(obj, beam.spec);

            // sound
            sound();
        }

    }
}
