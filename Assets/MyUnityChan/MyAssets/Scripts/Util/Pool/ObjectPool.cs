using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {

    public abstract class PoolObjectBase : ObjectBase {
        protected bool pooled = false;

        public abstract void initialize();
        public abstract void finalize();

        public void setPooled(bool flag) {
            pooled = flag;
        }

        public bool isPooledObject() {
            return pooled;
        }

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
                if ( objects[i] == null ) {
                    continue;
                }

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

        public int getObjectIndex(GameObject go) {
            return objects.Select((obj, index) => new { o = obj, i = index}).Where(v => v.o.gameObject == go).Select(v => v.i).FirstOrDefault();
        }

        private void initializeObject(GameObject go) {
            PoolObjectBase comp = go.GetComponent<PoolObjectBase>();
            if ( comp == null ) {
                Debug.LogError("Effect script is not found in effect prefab");
            }
            comp.setPooled(true);
            comp.initialize();
        }

        private void finalizeObject(GameObject go) {
            PoolObjectBase comp = go.GetComponent<PoolObjectBase>();
            comp.finalize();
        }
    }
}