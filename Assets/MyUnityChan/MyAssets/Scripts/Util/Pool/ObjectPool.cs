using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class PoolObjectBase : ObjectBase {
        public abstract void initialize();
        public abstract void finalize();
    }


    public class ObjectPool : ObjectBase {

        private List<PoolObjectBase> objects;
        private GameObject prefab;

        void Awake() {
            objects = new List<PoolObjectBase>();
        }

        public GameObject getGameObject() {
            GameObject go = null;

            for ( int i = 0; i < objects.Count; i++ ) {
                go = objects[i].gameObject;
                if ( !go.activeInHierarchy ) {
                    initializeObject(go);
                    go.SetActive(true);
                    return go;
                }
            }

            go = Instantiate(prefab) as GameObject;
            initializeObject(go);
            objects.Add(go.GetComponent<PoolObjectBase>());

            return go;
        }

        public void releaseGameObject(GameObject go) {
            finalizeObject(go);
            go.SetActive(false);
        }

        public void setPrefab(GameObject _prefab) {
            prefab = _prefab;
        }

        private void initializeObject(GameObject go) {
            go.GetComponent<PoolObjectBase>().initialize();
        }

        private void finalizeObject(GameObject go) {
            go.GetComponent<PoolObjectBase>().finalize();
        }
    }
}