using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MissileSupplyItem : SupplyItem {
        public int supply_num;

        public override void perform(Player player) {
            base.perform(player);

            MissilePod pod = player.GetComponent<MissilePod>();
            if ( pod ) pod.addMissile(supply_num);
        }

        public override void destroy(Player player) {
            base.destroy(player);

            if ( isPooledObject() ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, prefabPath(Const.ID.Item.MISSILE_SUPPLY));
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
