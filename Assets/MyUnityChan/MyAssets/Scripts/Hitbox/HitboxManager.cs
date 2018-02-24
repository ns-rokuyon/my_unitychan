using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class HitboxManager : PrefabManagerBase<HitboxManager> {
        public override string parent {
            get {
                return Hierarchy.Layout.HITBOX;
            }
        }

        public static T createHitbox<T>(Const.ID.Hitbox hitbox_id, bool use_objectpool = true) {
            return Instance.create<T>(ConfigTableManager.Hitbox.getPrefabConfig(hitbox_id).prefab,
                                      use_objectpool);
        }
    }
}
