using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class ProjectileSpec : AttackSpec {
        public int n_round_burst = 1;
        public int burst_delta_frame;
        public int interval_frame;
        public string hitbox_name;
        public bool copy_target_dir_to_force_degree = true;
        [SerializeField] public Const.ID.SE se_name;
        [SerializeField] public Const.ID.Effect shoot_effect = Const.ID.Effect._NO_EFFECT;

        public override Vector3 force_direction {
            get { return _force_direction; }
        }

        public override int regularizeDamage(int base_damage, Character character) {
            return base_damage + character.status.MATK;
        }

        public override void prepare(Hitbox hitbox) {
            _force_direction = (hitbox as ProjectileHitbox).projectile.GetComponent<Projectile>().target_dir;
        }

        public override void attack(Character character, Hitbox hitbox) {
            base.attack(character, hitbox);

            if ( character ) {
                if ( character.status.invincible.now() )
                    return;

                if ( character is IEnemyTakeProjectile )
                    (character as IEnemyTakeProjectile).onTakeProjectile(this);
            }
        }
    }
}
