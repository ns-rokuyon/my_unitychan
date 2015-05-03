using UnityEngine;
using System.Collections;

public static class Hierarchy {

    public static GameObject setParent(this GameObject self, GameObject parent) {
        self.transform.SetParent(parent.transform);
        return self;
    }

    public static GameObject setParent(this GameObject self, Component parent) {
        self.transform.SetParent(parent.gameObject.transform);
        return self;
    }

}
