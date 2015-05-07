using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Projectile : PoolObjectBase {
        protected Vector3 target_dir;
        protected Vector3 start_position;
        protected float distance_moved;
        protected float max_range;
        protected float speed;

        public virtual void setDir(Vector3 dir) {
            target_dir = dir;
        }

        public virtual void setStartPosition(Vector3 pos) {
            transform.position = pos;
            start_position = pos;
        }
    }
}
