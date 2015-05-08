using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class HitboxManager : SingletonObjectBase<HitboxManager> {
        private Dictionary<string, GameObject> prefabs;

        void Awake() {
            prefabs = new Dictionary<string, GameObject>();
        }

        private GameObject instantiatePrefab(string resource_path, string parent_object=null) {
            if ( !prefabs.ContainsKey(resource_path) ) {
                prefabs[resource_path] = Resources.Load(resource_path) as GameObject;
            }
            GameObject obj = Instantiate(prefabs[resource_path]) as GameObject;
            if ( parent_object != null ) {
                obj.setParent(parent_object);
            }

            return obj;
        }

        public T createHitbox<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T hitbox = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.HITBOX).GetComponent<T>();
                (hitbox as Hitbox).enablePool(resource_path);
                return hitbox;
            }
            return instantiatePrefab(resource_path, Hierarchy.Layout.HITBOX).GetComponent<T>();
        }
    }
}
