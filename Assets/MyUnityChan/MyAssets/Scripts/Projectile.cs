using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Projectile : ObjectBase {
        private Vector3 target_dir;
        private Vector3 start_position;
        private float distance_moved;
        private float max_range;
        private float speed;

        // Use this for initialization
        void Start() {
            distance_moved = 0.0f;
            max_range = 80.0f;
            speed = 1.0f;
        }

        // Update is called once per frame
        void Update() {
            transform.Translate(target_dir * speed, Space.World);
            distance_moved = Mathf.Abs(transform.position.x - start_position.x);
            if ( distance_moved > max_range ) {
                Destroy(this.gameObject);
            }
        }

        public void init(Vector3 start_pos, Vector3 dir, float sp = 1.0f) {
            target_dir = dir;
            transform.position = start_pos + target_dir * 2.0f + Vector3.up * 4.0f;
            start_position = transform.position;
            speed = sp;
        }
    }
}
