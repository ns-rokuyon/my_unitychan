using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class DamageObjectManager : PrefabManagerBase<DamageObjectManager> {
        public override string parent {
            get {
                return Hierarchy.Layout.DAMAGE_OBJECT;
            }
        }

        public static Bomb createBomb(Const.ID.Bomb bomb_id, bool use_objectpool = true) {
            return Instance.create<Bomb>(ConfigTableManager.Bomb.getPrefabConfig(bomb_id).prefab,
                                         use_objectpool);
        }
    }
}