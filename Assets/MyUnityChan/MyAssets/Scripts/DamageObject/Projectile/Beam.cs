using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Beam : Projectile  {
        private TrailRenderer trail;

        // Use this for initialization
        void Start() {
            trail = GetComponentInChildren<TrailRenderer>();
            initialize();
        }

        // Update is called once per frame
        void Update() {
            projectileCommonUpdate(Const.Prefab.Projectile[resource_name]);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos + 
                target_dir * start_position_offset.x + Vector3.up * start_position_offset.y + Vector3.forward * start_position_offset.z;
            start_position = transform.position;

            projectileCommonSetStartPosition();

            if ( trail != null ) {
                trail.reset(this);
            }
        }

        public override void initialize() {
            distance_moved = 0.0f;
            hit_num = 0;
        }

        public override void finalize() {
        }
    }

}