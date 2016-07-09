using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Boss_01 : Boss {

        protected override void start() {
            base.start();

            action_manager.registerAction(new EnemyDead(this, Const.Prefab.Effect["BLACK_EXPLOSION"], onDead));
            action_manager.registerAction(new EnemyLookAtPlayer(this));
            action_manager.registerAction(new EnemyJump(this, 0.0f, 1.5f));
        }

        private void onDead() {
            DropItem item = DropItemManager.self().create<DropItem>(Const.Prefab.Item["HP_RECOVERY"], true);
            item.setPosition(transform.position + Vector3.up);
        }

    }
}
