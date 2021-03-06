﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AttackHitbox : Hitbox {
        [SerializeField]
        public AttackSpec spec;

        public bool depend_on_parent_object { get; set; }

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

        public override void OnTriggerStay(Collider other) {
            if ( continuous_hit ) {
                if ( PauseManager.isPausing() )
                    return;
                System.Action func = () => {
                    triggerPlayer(other);
                    triggerEnemy(other);
                    triggerDoor(other);
                    triggerBlock(other);
                    triggerPhysicsObject(other);
                };
                if ( continuous_hit_interval_frame == 0 ) {
                    func();
                } else {
                    doPrevInterval("continuous_hit", continuous_hit_interval_frame, func);
                }
            }
        }

        protected bool triggerEnemy(Collider other) {
            if ( other.tag == "Enemy" ) {
                if ( isOwner(other.gameObject) ) return false;

                Enemy enemy = ((Enemy)other.gameObject.GetComponent<Enemy>());
                spec.prepare(this);
                spec.force(enemy, this);
                spec.playEffect(enemy, this);
                spec.playSound(enemy, this);
                spec.attack(enemy, this);
                return true;
            }
            return false;
        }

        protected bool triggerPlayer(Collider other) {
            if ( other.tag == "Player" ) {
                if ( isOwner(other.gameObject) ) return false;

                Player player = ((Player)other.gameObject.GetComponent<Player>());
                spec.prepare(this);
                spec.force(player, this);
                spec.playEffect(player, this);
                spec.playSound(player, this);
                spec.attack(player, this);
                return true;
            }
            return false;
        }

        protected bool triggerDoor(Collider other) {
            if ( other.tag == "Door" ) {
                Door door = ((Door)other.gameObject.GetComponent<Door>());
                door.open();
                spec.prepare(this);
                spec.playEffect(door, this);
                spec.playSound(door, this);
                spec.attack(null, this);
                return true;
            }
            return false;
        }

        protected bool triggerBlock(Collider other) {
            if ( other.tag == "Block" ) {
                Block block = ((Block)other.gameObject.GetComponent<Block>());
                spec.prepare(this);
                spec.playEffect(block, this);
                spec.playSound(block, this);
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
                    spec.prepare(this);
                    spec.force(o.rigid_body.rb, this);
                    spec.playEffect(o, this);
                    spec.playSound(o, this);
                }
                return true;
            }
            return false;
        }
    }
}
