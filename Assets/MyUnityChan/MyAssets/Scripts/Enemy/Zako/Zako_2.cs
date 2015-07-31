using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Zako_2 : Enemy {
        public Vector3 walk_f;
        public float walk_maxspeed;

        protected override void start() {
            action_manager = (Instantiate(enemy_action_manager_prefab) as GameObject).setParent(gameObject).GetComponent<EnemyActionManager>();
            action_manager.registerAction(new EnemyWalk(this, walk_f, walk_maxspeed));
            action_manager.registerAction(new EnemyDead(this, Const.Prefab.Effect["BLACK_EXPLOSION"], onDead));

            max_hp = 100;
            setHP(max_hp);
        }

        // callback when enemy is dying
        private void onDead() {
            DropItemManager.self().create<DropItem>(Const.Prefab.Item["HP_RECOVERY"], true).setPosition(transform.position + Vector3.up);
        }

        protected override void update() {
        }

        void FixedUpdate() {
        }
    }
}
