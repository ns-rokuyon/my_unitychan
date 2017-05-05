using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ProjectileHitbox : AttackHitbox {
        GameObject projectile;

        public override void ready(GameObject proj_, AttackSpec atkspec) {
            projectile = proj_;
            initPosition(atkspec);

            transform.position = projectile.transform.position;
        }

        protected override void UniqueUpdate() {
            if ( spec != null ) {
                if ( projectile != null && projectile.activeSelf ) {
                    transform.position = projectile.transform.position;
                }
                else {
                    destroy();
                }
            }
        }

        protected bool triggerGround(Collider other) {
            if ( other.tag == "Ground" ) {
                spec.attack(null, this);
                return true;
            }
            return false;
        }

        public override void OnTriggerEnter(Collider other) {
            Projectile proj = projectile.GetComponent<Projectile>();
            if ( triggerPlayer(other) ) proj.countHit();
            if ( triggerEnemy(other) ) proj.countHit();
            if ( triggerDoor(other) ) proj.countHit();
            if ( triggerGround(other) ) proj.countHit();
            if ( triggerBlock(other) ) proj.countHit();
        }

        /*
        public void OnTriggerEnter(Collider other) {
            if ( other.tag == "Enemy" ) {
                Enemy enemy = ((Enemy)other.gameObject.GetComponent<Enemy>());
                spec.attack(enemy, this);
                projectile.GetComponent<Projectile>().countHit();
            }
            else if ( other.tag == "Door" ) {
                Door door = ((Door)other.gameObject.GetComponent<Door>());
                door.open();
                projectile.GetComponent<Projectile>().countHit();
            }
        }
        */
    }
}
