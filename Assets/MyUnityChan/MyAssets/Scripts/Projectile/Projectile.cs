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
        protected string area_name;
        protected Player player;
        protected bool penetration;
        protected int hit_num = 0;

        public virtual void setDir(Vector3 dir) {
            target_dir = dir;
        }

        public bool isPenetration() {
            return penetration;
        }

        protected void commonSetStartPosition() {
            hit_num = 0;
            area_name = AreaManager.Instance.getAreaNameFromObject(this.gameObject);
        }

        public virtual void setStartPosition(Vector3 pos) {
            transform.position = pos;
            start_position = pos;

            commonSetStartPosition();
        }

        public void setPlayerInfo(Player _player) {
            player = _player;
        }

        protected void commonUpdate(string resource_path) {
            if ( PauseManager.isPausing() ) return;

            transform.Translate(target_dir * speed, Space.World);
            distance_moved = Mathf.Abs(transform.position.x - start_position.x);
            if ( distance_moved > max_range ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
            }
            if ( area_name == null || !AreaManager.Instance.isInArea(this.gameObject, area_name) ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
            }
            if ( !penetration && hit_num > 0 ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
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
