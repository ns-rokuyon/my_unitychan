using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ObjectPoolManager : SingletonObjectBase<ObjectPoolManager> {

        private Dictionary<GameObject, ObjectPool> pools;   // Key: prefab, Value: ObjectPool

        [SerializeField, ReadOnly]
        private List<GameObject> pooled_prefabs;

        void Awake() {
            pools = new Dictionary<GameObject, ObjectPool>();
            pooled_prefabs = new List<GameObject>();
        }

        public static void releaseGameObject(GameObject go) {
            var po = go.GetComponent<PoolObjectBase>();
            releaseGameObject(po);
        }

        public static void releaseGameObject(PoolObjectBase po) {
            Instance.pools[po.prefab].releaseGameObject(po.gameObject);
        }

        public static GameObject getGameObject(GameObject prefab) {
            if ( !Instance.hasPool(prefab) )
                Instance.addPool(prefab);
            return Instance.pools[prefab].getGameObject();
        }

        public static GameObject getGameObject(PoolObjectBase po) {
            return getGameObject(po.prefab);
        }

        public static ObjectPool getPool(GameObject prefab) {
            return Instance.pools[prefab];
        }

        // add new object pool for prefab
        public void addPool(GameObject prefab) {
            if ( !hasPool(prefab) ) {
                pools[prefab] = new ObjectPool(prefab);
                pooled_prefabs.Add(prefab);
            }
        }

        public bool hasPool(GameObject prefab) {
            return pools.ContainsKey(prefab);
        }
    }
}