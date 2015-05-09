using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class HitboxManager : PrefabManagerBase<HitboxManager> {

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T hitbox = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.HITBOX).GetComponent<T>();
                (hitbox as Hitbox).enablePool(resource_path);
                return hitbox;
            }
            return instantiatePrefab(resource_path, Hierarchy.Layout.HITBOX).GetComponent<T>();
        }
    }
}
