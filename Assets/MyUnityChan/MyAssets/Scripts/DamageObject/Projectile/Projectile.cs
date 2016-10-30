using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class Projectile : DamageObjectBase {
        [SerializeField] public ProjectileSpec spec;
        public bool penetration;
        public float speed;         // If 'use_physics' flag is true, 'speed' is not used
        public float max_range;
        public Vector3 start_position_offset = new Vector3(0.4f, 1.2f, 0.0f);

        protected Vector3 target_dir;
        protected Vector3 start_position;
        protected float distance_moved;
        protected string area;
        protected string area_name;
        protected Player player;
        protected int hit_num = 0;
        protected bool waiting_for_destroying;

        protected delegate void SetEnabledToComponent(bool f);
        protected List<SetEnabledToComponent> component_to_disable_in_waiting 
            = new List<SetEnabledToComponent>();

        public bool use_physics;
        public float waiting_time_for_destroying = 0.0f;
        public string resource_name;

        public virtual void setDir(Vector3 dir) {
            target_dir = dir;
        }

        public bool isPenetration() {
            return penetration;
        }

        protected void projectileCommonSetStartPosition() {
            hit_num = 0;
            area_name = AreaManager.Instance.getAreaNameFromObject(this.gameObject);
            waiting_for_destroying = false;

            EffectManager.self().createEffect(spec.shoot_effect, start_position, 60, true);
        }

        public override void setStartPosition(Vector3 pos) {
            transform.position = pos;
            start_position = pos;

            projectileCommonSetStartPosition();
        }

        public void setPlayerInfo(Player _player) {
            player = _player;
        }

        protected void projectileCommonUpdate() {
            projectileCommonUpdate(Const.Prefab.Projectile[resource_name]);
        }

        protected void projectileCommonUpdate(string resource_path) {
            if ( PauseManager.isPausing() ) return;

            if ( waiting_for_destroying ) return;

            if ( !use_physics ) transform.Translate(target_dir * speed, Space.World);

            distance_moved = Mathf.Abs(transform.position.x - start_position.x);
            if ( distance_moved > max_range ) {
                StartCoroutine("destroy", resource_path);
            }
            else if ( area_name == null || !AreaManager.Instance.isInArea(this.gameObject, area_name) ) {
                StartCoroutine("destroy", resource_path);
            }
            else if ( !penetration && hit_num > 0 ) {
                StartCoroutine("destroy", resource_path);
            }
        }

        protected IEnumerator destroy(string resource_path) {
            if ( waiting_time_for_destroying > 0.0f ) {
                if ( GetComponent<Rigidbody>() ) GetComponent<Rigidbody>().velocity = Vector3.zero;
                foreach ( var component_enabler in component_to_disable_in_waiting ) {
                    component_enabler(false);
                }
                waiting_for_destroying = true;
                yield return new WaitForSeconds(waiting_time_for_destroying);
            }
            ObjectPoolManager.releaseGameObject(gameObject, resource_path);
        }

        public void countHit() {
            hit_num++;
        }

        public int getHitNum() {
            return hit_num;
        }

    }
}
