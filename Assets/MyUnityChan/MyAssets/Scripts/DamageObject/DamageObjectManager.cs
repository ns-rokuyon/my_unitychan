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

        public static T createDamageObject<T>(Const.ID.DamageObject damageobject_id, bool use_objectpool = true) {
            return Instance.create<T>(ConfigTableManager.DamageObject.getPrefabConfig(damageobject_id).prefab,
                                      use_objectpool);
        }
    }
}