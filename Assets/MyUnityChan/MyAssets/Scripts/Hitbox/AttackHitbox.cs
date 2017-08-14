using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AttackHitbox : Hitbox {
        [SerializeField]
        public AttackSpec spec;

        protected void initPosition(AttackSpec atkspec) {
            // attack parameter
            spec = atkspec;

            // timer
            startCountdown(spec.frame);
        }

        protected void initPosition(Vector3 pos, Vector3 fw, AttackSpec atkspec) {
            // position
            transform.position = pos;
            forward = fw;

            // attack parameter
            spec = atkspec;

            // timer
            startCountdown(spec.frame);
        }

        public override void OnTriggerEnter(Collider other) {
            // 'other' object must be attached Colider and Rigidbody component
            triggerPlayer(other);
            triggerEnemy(other);
            triggerDoor(other);
            triggerBlock(other);
            triggerPhysicsObject(other);
        }

        protected bool triggerEnemy(Collider other) {
            if ( other.tag == "Enemy" ) {
                if ( isOwner(other.gameObject) ) return false;

                Enemy enemy = ((Enemy)other.gameObject.GetComponent<Enemy>());
                DebugManager.log("hit to " + enemy.name);
                spec.attack(enemy, this);
                spec.force(enemy.rigid_body.rb, this);
                return true;
            }
            return false;
        }

        protected bool triggerPlayer(Collider other) {
            if ( other.tag == "Player" ) {
                if ( isOwner(other.gameObject) ) return false;

                Player player = ((Player)other.gameObject.GetComponent<Player>());
                spec.attack(player, this);
                return true;
            }
            return false;
        }

        protected bool triggerDoor(Collider other) {
            if ( other.tag == "Door" ) {
                Door door = ((Door)other.gameObject.GetComponent<Door>());
                door.open();
                spec.attack(null, this);
                return true;
            }
            return false;
        }

        protected bool triggerBlock(Collider other) {
            if ( other.tag == "Block" ) {
                Block block = ((Block)other.gameObject.GetComponent<Block>());
                spec.attack(null, this);
                block.damage(spec.damage);
                return true;
            }
            return false;
        }

        protected bool triggerPhysicsObject(Collider other) {
            if ( other.tag == "PhysicsObject" ) {
                ObjectBase o = other.gameObject.GetComponent<ObjectBase>();
                if ( o ) {
                    spec.force(o.rigid_body.rb, this);
                }
            }
            return false;
        }
    }
}
