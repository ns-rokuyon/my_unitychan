﻿using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ObjectPoolManager : SingletonObjectBase<ObjectPoolManager> {

        private Dictionary<string, ObjectPool> pools;

        void Awake() {
            pools = new Dictionary<string, ObjectPool>();
        }

        // Use this for initialization
        void Start() {
            // To manage object using ObjectPool, add calling addPool method here
            addPool(Beam.resource_path);
            addPool(Hadouken.resource_path);
            addPool(PlayerHadouken.hitbox_resource_path);
        }

        public static void releaseGameObject(GameObject go, string resource_path) {
            self().pools[resource_path].releaseGameObject(go);
        }

        public static GameObject getGameObject(string resource_path) {
            return self().pools[resource_path].getGameObject();
        }

        // add new object pool for prefab in resource_path
        private bool addPool(string resource_path) {
            GameObject prefab = Resources.Load(resource_path) as GameObject;
            if ( !prefab ) {
                Debug.LogError("prefab notfound: " + resource_path);
                return false;
            }

            if ( pools.ContainsKey(resource_path) ) {
                Debug.LogError("object pool already exists (key=" + resource_path + ")");
                return false;
            }

            GameObject pool = new GameObject("ObjectPool_" + prefab.name);
            pool.setParent(Hierarchy.Layout.OBJECT_POOL);
            pools[resource_path] = pool.AddComponent<ObjectPool>();
            pools[resource_path].setPrefab(prefab);

            return true;
        }

    }
}