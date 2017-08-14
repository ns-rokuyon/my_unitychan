using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public partial class Misc {
        public static Vector3 degree2DirectionVector3(float degree) {
            return new Vector3(Mathf.Cos(degree), Mathf.Sin(degree), 0.0f);
        }
    }
}