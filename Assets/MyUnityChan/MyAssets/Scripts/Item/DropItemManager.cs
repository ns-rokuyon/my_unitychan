using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class DropItemManager : PrefabManagerBase<DropItemManager> {
        public override string parent {
            get {
                return Hierarchy.Layout.DROP_ITEM;
            }
        }

        public static T createItem<T>(Const.ID.Item item_name, bool use_objectpool = true) {
            return Instance.create<T>(ConfigTableManager.Item.getPrefabConfig(item_name).prefab,
                                      use_objectpool);
        }
    }
}
