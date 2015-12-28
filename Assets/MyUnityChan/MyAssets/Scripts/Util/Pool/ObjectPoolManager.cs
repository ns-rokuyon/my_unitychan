using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ObjectPoolManager : SingletonObjectBase<ObjectPoolManager> {

        private Dictionary<string, ObjectPool> pools;

        void Awake() {
            pools = new Dictionary<string, ObjectPool>();
            // To manage object using ObjectPool, add calling addPool method here
            addPool(Const.Prefab.Projectile["NORMAL_BEAM"]);
            addPool(Const.Prefab.Projectile["NORMAL_BEAM_02"]);
            addPool(Const.Prefab.Projectile["FLAME"]);
            addPool(Const.Prefab.Projectile["HADOUKEN"]);
            addPool(Const.Prefab.Projectile["BEAM_01"]);
            addPool(Const.Prefab.Hitbox["PROJECTILE"]);
            addPool(Const.Prefab.Hitbox["BEAM"]);
            addPool(Const.Prefab.Hitbox["FLAME"]);
            addPool(Const.Prefab.Effect["BLACK_EXPLOSION"]);
            addPool(Const.Prefab.Effect["HIT_01"]);
            addPool(Const.Prefab.Effect["HIT_02"]);
            addPool(Const.Prefab.Effect["GUARD_01"]);
            addPool(Const.Prefab.Effect["JUMP_SMOKE_PUFF"]);
            addPool(Const.Prefab.Item["HP_RECOVERY"]);
            addPool(Const.Prefab.Timer["FRAME_TIMER"]);
        }

        // Use this for initialization
        void Start() {
        }

        public static void releaseGameObject(GameObject go, string resource_path) {
            self().pools[resource_path].releaseGameObject(go);
        }

        public static GameObject getGameObject(string resource_path) {
            return self().pools[resource_path].getGameObject();
        }

        public static int getObjectIndex(GameObject go, string resource_path) {
            return self().pools[resource_path].getObjectIndex(go);
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