using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
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

        public static T[] GetComponentsInShallowChildren<T>(this GameObject self, int depth = 1) {
            if ( depth == 0 ) {
                return new T[0];
            }

            List<T> ts = new List<T>();
            foreach ( Transform tf in self.transform ) {
                T t = tf.gameObject.GetComponent<T>();
                if ( t == null )
                    continue;
                ts.Add(t);
                ts.AddRange(tf.gameObject.GetComponentsInShallowChildren<T>(depth - 1));
            }

            return ts.ToArray();
        }

        public static T[] GetComponentsInShallowChildren<T>(this UnityEngine.Component self, int depth = 1) {
            return self.gameObject.GetComponentsInShallowChildren<T>(depth);
        }

        public static T[] GetComponentsInShallowChildren<T>(this UnityEngine.MonoBehaviour self, int depth = 1) {
            return self.gameObject.GetComponentsInShallowChildren<T>(depth);
        }
    }
}
