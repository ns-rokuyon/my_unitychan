using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyDead : EnemyActionBase {
        public override string name() {
            return "ENEMY_DEAD";
        }

        public EnemyDead(Character character) : base(character) {
        }

        public override void perform() {
            enemy.setHP(0);

            // OnDead behavior
            if ( enemy is IEnemyDead ) {
                IEnemyDead en = enemy as IEnemyDead;
                en.onDead();
                en.createDeadEffect();
            }

            // DropItem
            if ( enemy is IEnemyItemDrop ) {
                Const.ID.Item itemname = (enemy as IEnemyItemDrop).dropItem();
                DropItem item = DropItemManager.createItem<DropItem>(itemname, true);
                item.setPosition(enemy.transform.position + Vector3.up);
            }

            // Discard
            enemy.destroyHpGauge();
            enemy.gameObject.SetActive(false);
        }

        public override bool condition() {
            return enemy.getHP() <= 0;
        }

    }

}