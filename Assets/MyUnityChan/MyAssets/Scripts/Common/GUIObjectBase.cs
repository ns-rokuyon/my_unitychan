using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GUIObjectBase : ObjectBase {
        public static string canvas_name = "Canvas";

        public static GameObject getCanvas() {
            return GameObject.Find(canvas_name);
        }
    }
}
