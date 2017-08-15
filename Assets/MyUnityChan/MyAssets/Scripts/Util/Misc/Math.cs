using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public partial class Misc {
        public static Vector3 degree2DirectionVector3(float degree) {
            return radian2DirectionVector3(Mathf.Deg2Rad * degree);
        }

        public static Vector3 radian2DirectionVector3(float radian) {
            return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0.0f);
        }
    }
}