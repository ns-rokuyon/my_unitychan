using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class BeamTurret : TurretBase {
        public Vector3 base_angle = new Vector3(1.0f, 0.0f, 0.0f);
        public string beam_name;

        void Start() {
            baseStart();
            setBeam(beam_name);
        }

        public void setBeam(string name) {
            BeamSpec spec = (Resources.Load(Const.Prefab.Projectile[name]) as GameObject)
                .GetComponent<Beam>().spec;

            shooting_frame = spec.shooting_frame;
            interval_frame = spec.interval_frame;
            se_name = spec.se_name;
            hitbox_name = spec.hitbox_name;
        }

        public override Vector3 angle() {
            if ( this.gameObject.transform.forward.x >= 0 ) return base_angle;
            return base_angle.flipX();
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
