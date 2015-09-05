using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public static class Component {
        public static GameObject removeComponent<T>(this GameObject self) where T : UnityEngine.Component {
            GameObject.Destroy(self.GetComponent<T>());
            return self;
        }

    }
}
