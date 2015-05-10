using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EffectManager : PrefabManagerBase<EffectManager> {

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T effect = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.EFFECT).GetComponent<T>();
                (effect as Hitbox).enablePool(resource_path);
                return effect;
            }
            return instantiatePrefab(resource_path, Hierarchy.Layout.EFFECT).GetComponent<T>();
        }
    }
}
