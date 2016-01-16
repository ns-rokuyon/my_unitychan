using UnityEngine;
using System.Collections;

public static class Vector {

    public static Vector3 flipX(this Vector3 vec) {
        return new Vector3(-vec.x, vec.y, vec.z);
    }

    public static Vector3 add(this Vector3 vec, float diff_x, float diff_y, float diff_z) {
        return new Vector3(vec.x + diff_x, vec.y + diff_y, vec.z + diff_z);
    }

    public static Vector3 mul(this Vector3 vec, float mx, float my, float mz) {
        return new Vector3(vec.x * mx, vec.y * my, vec.z * mz);
    }

    public static Vector3 changeX(this Vector3 vec, float new_x) {
        return new Vector3(new_x, vec.y, vec.z);
    }

    public static Vector3 changeY(this Vector3 vec, float new_y) {
        return new Vector3(vec.x, new_y, vec.z);
    }

    public static Vector3 changeZ(this Vector3 vec, float new_z) {
        return new Vector3(vec.x, vec.y, new_z);
    }
}
