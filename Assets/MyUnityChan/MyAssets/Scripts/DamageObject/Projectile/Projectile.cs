﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class Projectile : ShootableObject {
        public abstract class Custom : ObjectBase {
            public abstract void initialize();
            public abstract void finalize();
        }

        [SerializeField] public ProjectileSpec spec;
        public bool penetration;
        public float speed;             // If 'use_physics' flag is true, 'speed' is not used
        public float acceleration;      // If 'use_physics' flag is true, 'acceleration' is not used
        public float terminal_velocity; // If acceleration > 0.0f, stop accelerating when speed reaches terminal velocity
        public float max_range;
        public Vector3 start_position_offset = new Vector3(0.4f, 1.2f, 0.0f);

        protected Vector3 start_position;
        protected float distance_moved;
        protected string area;
        protected string area_name;
        protected Player player;
        protected int hit_num = 0;

        public bool use_physics;
        public string resource_name;

        public Vector3 target_dir { get; set; }
        public float velocity { get; set; }         // Velocity > 0

        public virtual void setDir(Vector3 dir) {
            //transform.rotation = Quaternion.LookRotation(dir);
            target_dir = dir;
        }

        public bool isPenetration() {
            return penetration;
        }

        protected void projectileCommonSetStartPosition() {
            hit_num = 0;
            velocity = speed;       // Initial velocity
            area_name = AreaManager.Instance.getAreaNameFromObject(this.gameObject);
            waiting_for_destroying = false;

            EffectManager.createEffect(spec.shoot_effect, start_position, 60, true);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos;
            start_position = pos;

            projectileCommonSetStartPosition();
        }

        public void setPlayerInfo(Player _player) {
            player = _player;
        }

        protected void projectileCommonUpdate() {
            projectileCommonUpdate(Const.Prefab.Projectile[resource_name]);
        }

        protected void projectileCommonUpdate(string resource_path) {
            if ( waiting_for_destroying ) return;

            if ( !use_physics ) {
                if ( acceleration > 0.0f ) {
                    velocity = velocity + acceleration * time_control.deltaTime;
                    if ( terminal_velocity > 0.0f ) {
                        velocity = terminal_velocity;
                    }
                }
                transform.Translate(target_dir * velocity * time_control.deltaTime, Space.World);
            }

            distance_moved = Mathf.Abs(transform.position.x - start_position.x);
            if ( distance_moved > max_range ) {
                StartCoroutine("destroy", resource_path);
            }
            else if ( area_name == null || !AreaManager.Instance.isInArea(this.gameObject, area_name) ) {
                StartCoroutine("destroy", resource_path);
            }
            else if ( !penetration && hit_num > 0 ) {
                StartCoroutine("destroy", resource_path);
            }
        }

        public void countHit() {
            hit_num++;
        }

        public int getHitNum() {
            return hit_num;
        }
    }
}
