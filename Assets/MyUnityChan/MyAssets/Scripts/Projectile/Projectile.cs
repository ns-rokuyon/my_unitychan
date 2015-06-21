using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Projectile : PoolObjectBase {
        protected Vector3 target_dir;
        protected Vector3 start_position;
        protected float distance_moved;
        protected float max_range;
        protected float speed;
        protected string area;
        protected Player player;

        public virtual void setDir(Vector3 dir) {
            target_dir = dir;
        }

        public virtual void setStartPosition(Vector3 pos) {
            transform.position = pos;
            start_position = pos;
        }

        public void setPlayerInfo(Player _player) {
            area = _player.getAreaName();
            player = _player;
        }

        protected void commonUpdate(string resource_path) {
            transform.Translate(target_dir * speed, Space.World);
            distance_moved = Mathf.Abs(transform.position.x - start_position.x);
            if ( distance_moved > max_range ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
            }
            if ( area != player.getAreaName() ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
            }
        }

    }
}
