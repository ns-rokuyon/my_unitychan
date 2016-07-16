using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DropItemManager : PrefabManagerBase<DropItemManager> {

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T item = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.DROP_ITEM).GetComponent<T>();
                return item;
            }
            return instantiatePrefab(resource_path, Hierarchy.Layout.DROP_ITEM).GetComponent<T>();
        }

        public static T createItem<T>(Const.ID.Item item_name, bool use_objectpool = false) {
            return Instance.create<T>(Const.Prefab.Item[item_name], use_objectpool);
        }
    }
}
