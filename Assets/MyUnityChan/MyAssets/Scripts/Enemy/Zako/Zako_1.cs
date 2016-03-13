using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Zako_1 : Enemy {

        protected override void start() {
            action_manager.registerAction(new EnemyWalk(this, new Vector3(20.0f, 0, 0), 1.0f));
            action_manager.registerAction(new EnemyDead(this, Const.Prefab.Effect["BLACK_EXPLOSION"], onDead));

            max_hp = 100;
            setHP(max_hp);
        }

        // callback when enemy is dying
        private void onDead() {
            string itemname;
            if ( Random.value > 0.5 ) {
                itemname = "HP_RECOVERY";
            } else {
                itemname = "MISSILE_SUPPLY";
            }

            DropItem item = DropItemManager.self().create<DropItem>(Const.Prefab.Item[itemname], true);
            item.setPosition(transform.position + Vector3.up);
        }

        protected override void update() {
        }

        void FixedUpdate() {
        }
    }
}