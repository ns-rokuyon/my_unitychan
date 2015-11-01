using UnityEngine;
using System.Collections;

public static class Vector {

    public static Vector3 flipX(this Vector3 vec) {
        return new Vector3(-vec.x, vec.y, vec.z);
    }

}
