using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class HpRecoveryItem : RecoveryItem {
        public int heal;

        public override void perform(Player player) {
            base.perform(player);
            
            player.setHP(player.getHP() + heal);
            EffectManager.createEffect(Const.ID.Effect.HEAL_01, player.gameObject, 0.0f, 0.5f, 60, true);
        }

        public override void destroy(Player player) {
            base.destroy(player);

            if ( isPooledObject() ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, prefabPath(Const.ID.Item.HP_RECOVERY));
            }
            else {
                Destroy(this.gameObject);
            }
        }

        public override void initialize() {
            base.initialize();
        }

        public override void finalize() {
            base.finalize();
        }
    }
}
