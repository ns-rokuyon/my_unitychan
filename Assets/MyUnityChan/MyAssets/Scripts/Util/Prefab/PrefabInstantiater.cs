using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PrefabInstantiater : SingletonObjectBase<PrefabInstantiater> {
        public static GameObject create(string resource_path, string parent = null) {
            GameObject obj = (Instantiate(Resources.Load(resource_path)) as GameObject);
            if ( parent != null ) {
                obj.setParent(parent);
            }
            return obj;
        }

        public static T createAndGetComponent<T>(string resource_path, string parent = null) {
            return create(resource_path, parent).GetComponent<T>();
        }
    }
}
