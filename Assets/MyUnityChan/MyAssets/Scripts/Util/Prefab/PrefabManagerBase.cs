using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class PrefabManagerBase<T> : SingletonObjectBase<PrefabManagerBase<T>> {
        protected Dictionary<string, GameObject> prefabs;

        void Awake() {
            prefabs = new Dictionary<string, GameObject>();
        }

        public abstract T create<T>(string resource_path, bool use_objectpool = false);

        protected GameObject instantiatePrefab(string resource_path, string parent_object=null) {
            if ( !prefabs.ContainsKey(resource_path) ) {
                prefabs[resource_path] = Resources.Load(resource_path) as GameObject;
            }
            GameObject obj = Instantiate(prefabs[resource_path]) as GameObject;
            if ( parent_object != null ) {
                obj.setParent(parent_object);
            }

            return obj;
        }

    }
}