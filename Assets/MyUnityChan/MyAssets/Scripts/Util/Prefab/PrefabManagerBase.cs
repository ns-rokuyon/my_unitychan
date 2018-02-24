using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class PrefabManagerBase<T> : SingletonObjectBase<PrefabManagerBase<T>> {
        public abstract string parent {
            get;
        }

        public virtual U create<U>(GameObject prefab, bool use_objectpool=true) {
            GameObject obj;
            if ( use_objectpool )
                obj = ObjectPoolManager.getGameObject(prefab);
            else
                obj = PrefabInstantiater.create(prefab, parent);

            return obj.setParent(parent).GetComponent<U>();
        }
    }
}