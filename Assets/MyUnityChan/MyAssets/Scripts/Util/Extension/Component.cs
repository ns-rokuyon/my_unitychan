using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public static class Component {
        public static GameObject removeComponent<T>(this GameObject self) where T : UnityEngine.Component {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(self.GetComponent<T>());
#else
            GameObject.Destroy(self.GetComponent<T>());
#endif
            return self;
        }

        public static T[] GetComponentsInSameArea<T>(this GameObject self) {
            ObjectBase ob = self.GetComponent<ObjectBase>();
            if ( ob == null || ob.parent_area == null ) {
                return new T[0];
            }

            return ob.parent_area.gameobjects.Values.ToList()
                .Where(o => o != null && o.GetComponent<T>() != null)
                .Select(o => o.GetComponent<T>())
                .ToArray();
        }

        public static T[] GetComponentsInSameArea<T>(this UnityEngine.Component self) {
            return self.gameObject.GetComponentsInSameArea<T>();
        }
    }
}
