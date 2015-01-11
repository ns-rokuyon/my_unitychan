using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ProjectileHitbox : AttackHitbox {
        GameObject projectile;

        public void create(GameObject proj_, AttackSpec atkspec) {
            projectile = proj_;
            initPosition(atkspec);

            transform.position = projectile.transform.position;
        }

        protected override void UniqueUpdate() {
            if ( spec != null ) {
                if ( projectile != null ) {
                    transform.position = projectile.transform.position;
                }
                else {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
