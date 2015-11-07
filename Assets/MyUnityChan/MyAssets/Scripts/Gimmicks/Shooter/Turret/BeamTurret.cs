using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class BeamTurret : TurretBase {
        public Vector3 base_angle = new Vector3(1.0f, 0.0f, 0.0f);
        public string beam_name = "BEAM";

        private AttackSpec spec;

        void Start() {
            baseStart();
            spec = new Beam.Spec();
            spec.stun = 0;
        }

        public override Vector3 angle() {
            if ( this.gameObject.transform.forward.x >= 0 ) return base_angle;
            return base_angle.flipX();
        }

        public override void shoot() {
            GameObject beam = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[beam_name]);
            beam.setParent(Hierarchy.Layout.PROJECTILE);

            Beam prjc = beam.GetComponent<Beam>();
            prjc.setDir(angle());
            prjc.setStartPosition(this.gameObject.transform.position);

            // hitbox
            ProjectileHitbox hitbox = HitboxManager.self().create<ProjectileHitbox>(Const.Prefab.Hitbox["BEAM"], use_objectpool:true);
            hitbox.setOwner(this.gameObject);
            hitbox.ready(beam, spec);
        }

    }
}
