using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class BeamTurret : TurretBase {
        [SerializeField]
        public Const.BeamName beam_name;

        void Start() {
            baseStart();
            //setProjectile(beam_name.ToString());

            this.ObserveEveryValueChanged(_ => beam_name)
                .Where(bn => bn != Const.BeamName._NOT_SET)
                .Subscribe(bn => setProjectile(bn.ToString()))
                .AddTo(gameObject);
        }

        public override void shoot() {
            if ( beam_name == Const.BeamName._NOT_SET )
                return;

            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile[projectile_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Beam beam = obj.GetComponent<Beam>();
            beam.time_control.changeClock(owner.time_control.clockName);
            beam.setDir(direction);
            beam.setStartPosition(this.gameObject.transform.position);

            // hitbox
            ProjectileHitbox hitbox;
            if ( beam.has_hitbox_in_children ) {
                hitbox = beam.gameObject.GetComponentInChildren<ProjectileHitbox>();
                hitbox.setEnabledCollider(true);
                hitbox.depend_on_parent_object = true;
            }
            else {
                hitbox = HitboxManager.self().create<ProjectileHitbox>(Const.Prefab.Hitbox[hitbox_name], use_objectpool:true);
            }
            hitbox.setOwner(this.gameObject);
            hitbox.ready(obj, beam.spec);

            // sound
            sound();
        }

        public void switchBeam(Const.BeamName _beam) {
            beam_name = _beam;
        }
    }
}
