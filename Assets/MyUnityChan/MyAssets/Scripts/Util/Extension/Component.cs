using UnityEngine;

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

    }
}
