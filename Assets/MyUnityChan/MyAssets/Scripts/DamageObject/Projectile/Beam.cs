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
            transform.position = pos + target_dir * 0.4f + Vector3.up * 1.2f;
            start_position = transform.position;

            projectileCommonSetStartPosition();

            if ( trail != null ) {
                trail.reset(this);
            }
        }

        public override void initialize() {
            penetration = false;
            distance_moved = 0.0f;
            max_range = 40.0f;
            speed = 0.2f;
            hit_num = 0;
        }

        public override void finalize() {
        }
    }

}