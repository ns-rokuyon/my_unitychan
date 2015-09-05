using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GUIObjectBase : ObjectBase {
        public static GameObject getCanvas(string canvas_name) {
            return GameObject.Find(canvas_name);
        }
    }
}
