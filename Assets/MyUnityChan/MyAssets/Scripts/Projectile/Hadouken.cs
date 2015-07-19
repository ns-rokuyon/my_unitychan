using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Hadouken : Projectile {
        public static readonly string resource_path = Const.Prefab.Projectile["HADOUKEN"];

        // Use this for initialization
        void Start() {
            distance_moved = 0.0f;
            max_range = 40.0f;
            speed = 0.2f;
        }

        // Update is called once per frame
        void Update() {
            commonUpdate(resource_path);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos + target_dir * 0.4f + Vector3.up * 0.8f;
            start_position = transform.position;

            commonSetStartPosition();
        }

        public override void initialize() {
        }

        public override void finalize() {
        }
    }
}