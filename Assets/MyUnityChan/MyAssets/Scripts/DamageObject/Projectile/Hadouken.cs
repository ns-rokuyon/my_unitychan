using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Hadouken : Projectile {
        public static readonly string resource_path = Const.Prefab.Projectile["HADOUKEN"];

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            projectileCommonUpdate(resource_path);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos + target_dir * 0.4f + Vector3.up * 0.8f;
            start_position = transform.position;

            projectileCommonSetStartPosition();
        }

        public override void initialize() {
            penetration = true;
            hit_num = 0;
            distance_moved = 0.0f;
        }

        public override void finalize() {
        }
    }
}