using UnityEngine;
using System.Collections.Generic;

public static class Hierarchy {

    // Method Extension
    public static GameObject setParent(this GameObject self, string parent) {
        if ( !roots.ContainsKey(parent) ) {
            GameObject found = GameObject.Find(parent);
            if ( found ) {
                roots[parent] = found;
            }
            else {
                Debug.LogError("parent object is not found: " + parent);
            }
        }
        self.transform.SetParent(roots[parent].transform);
        return self;
    }

    // Method Extension
    [System.Obsolete("Deprecated")]
    public static GameObject setParent(this GameObject self, GameObject parent) {
        self.transform.SetParent(parent.transform);
        return self;
    }

    // Method Extension
    [System.Obsolete("Deprecated")]
    public static GameObject setParent(this GameObject self, Component parent) {
        self.transform.SetParent(parent.gameObject.transform);
        return self;
    }

    public static class Layout {
        public static readonly string TIMER = "System/Timer";
        public static readonly string INVOKER = "System/Invoker";
        public static readonly string PROJECTILE = "Object/Projectile";
        public static readonly string PARTICLE = "Object/Particle";
    }

    public static Dictionary<string, GameObject> roots = new Dictionary<string, GameObject>();

}