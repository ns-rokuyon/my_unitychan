using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class BeamTurret : TurretBase {
        [SerializeField]
        public Const.ID.Projectile.Beam beam_id;

        public override void Start() {
            base.Start();

            this.ObserveEveryValueChanged(_ => beam_id)
                .Where(bid => bid != Const.ID.Projectile.Beam._NO_SET)
                .Subscribe(bid => setProjectile(bid))
                .AddTo(gameObject);
        }

        public override void shoot() {
            if ( beam_id == Const.ID.Projectile.Beam._NO_SET )
                return;

            Beam beam = ProjectileManager.createBeam<Beam>(beam_id);
            beam.time_control.changeClock(owner.time_control.clockName);
            beam.setDir(direction);
            beam.setStartPosition(this.gameObject.transform.position + muzzle_offset);
            beam.linkShooter(this);

            // hitbox
            ProjectileHitbox hitbox;
            if ( beam.has_hitbox_in_children ) {
                hitbox = beam.gameObject.GetComponentInChildren<ProjectileHitbox>();
                hitbox.setEnabledCollider(true);
                hitbox.depend_on_parent_object = true;
            }
            else {
                hitbox = HitboxManager.createHitbox<ProjectileHitbox>(hitbox_id);
            }
            hitbox.setOwner(this.gameObject);
            hitbox.ready(beam.gameObject, beam.spec);

            // sound
            sound();
        }

        public void setProjectile(Const.ID.Projectile.Beam id) {
            beam_id = id;
            Projectile proj = (Resources.Load(Const.Prefab.Projectile.Beam[beam_id]) as GameObject).GetComponent<Projectile>();
            ProjectileSpec spec = proj.spec;

            n_round_burst = spec.n_round_burst;
            burst_delta_frame = spec.burst_delta_frame;
            interval_frame = spec.interval_frame;
            se_id = spec.se_id;
            hitbox_id = spec.hitbox_id;
        }


        public void switchBeam(Const.ID.Projectile.Beam id) {
            beam_id = id;
        }
    }
}
