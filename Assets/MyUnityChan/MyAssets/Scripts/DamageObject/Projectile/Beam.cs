using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Beam : Projectile  {
        public Const.ID.Projectile.Beam resource_name;

        private TrailRenderer trail;

        // Use this for initialization
        void Start() {
            trail = GetComponentInChildren<TrailRenderer>();
            initialize();
        }

        // Update is called once per frame
        void Update() {
            projectileCommonUpdate();
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
            foreach ( var c in GetComponents<Projectile.Custom>() ) {
                c.initialize();
            }
        }

        public override void finalize() {
            foreach ( var c in GetComponents<Projectile.Custom>() ) {
                c.finalize();
            }
        }
    }

}