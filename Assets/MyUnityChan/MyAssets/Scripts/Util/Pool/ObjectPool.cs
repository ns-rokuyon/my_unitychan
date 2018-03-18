using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class PoolObjectBase : ObjectBase {
        // Base prefab reference for this object
        public GameObject prefab;

        public bool pooled { get; set; }

        public abstract void initialize();
        public abstract void finalize();
    }


    public class ObjectPool {
        public bool debug { get; set; }
        public GameObject prefab { get; private set; }  // Original

        private List<PoolObjectBase> objects;

        public ObjectPool(GameObject _prefab) {
            objects = new List<PoolObjectBase>();
            prefab = _prefab;
        }

        public GameObject getGameObject() {
            GameObject go = null;

            for ( int i = 0; i < objects.Count; i++ ) {
                if ( objects[i] == null ) {
                    continue;
                }

                go = objects[i].gameObject;
                if ( !go.activeInHierarchy ) {
                    go.SetActive(true);
                    initializeObject(go);
                    debugLog("Return pooled object (i=" + i + "/" + objects.Count + ")");
                    return go;
                }
            }

            go = PrefabInstantiater.create(prefab);
            initializeObject(go);
            objects.Add(go.GetComponent<PoolObjectBase>());

            debugLog("Return new pooled object (pooled=" + objects.Count + ")");

            return go;
        }

        public void releaseGameObject(GameObject go) {
            finalizeObject(go);
            go.SetActive(false);
        }

        public void setPrefab(GameObject _prefab) {
            prefab = _prefab;
        }

        public int getObjectIndex(GameObject go) {
            return objects.Select((obj, index) => new { o = obj, i = index}).Where(v => v.o.gameObject == go).Select(v => v.i).FirstOrDefault();
        }

        private void initializeObject(GameObject go) {
            PoolObjectBase comp = go.GetComponent<PoolObjectBase>();
            if ( comp == null ) {
                DebugManager.log("No PoolObjectBase attached: " + go, Const.Loglevel.ERROR);
            }
            comp.prefab = prefab;
            comp.managed_by_objectpool = true;
            comp.pooled = true;
            comp.initialize();
        }

        private void finalizeObject(GameObject go) {
            PoolObjectBase comp = go.GetComponent<PoolObjectBase>();
            comp.finalize();
        }

        private void debugLog(string message) {
            if ( debug )
                DebugManager.log("ObjectPool[" + prefab.name + "]: " + message);
        }
    }
}