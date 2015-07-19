using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Beam : Projectile  {
        public static readonly string resource_path = Const.Prefab.Projectile["BEAM"];

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            commonUpdate(resource_path);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos + target_dir * 0.4f + Vector3.up * 1.2f;
            start_position = transform.position;

            commonSetStartPosition();
        }

        public override void initialize() {
            distance_moved = 0.0f;
            max_range = 40.0f;
            speed = 0.2f;
        }

        public override void finalize() {
        }
    }

}