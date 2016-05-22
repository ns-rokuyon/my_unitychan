using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class GUIObjectBase : ObjectBase {
        public static Dictionary<string, GameObject> canvases = new Dictionary<string, GameObject>();

        public static GameObject getCanvas(string canvas_name) {
            if ( canvases.ContainsKey(canvas_name) )
                return canvases[canvas_name];

            GameObject canvas = GameObject.Find(canvas_name);
            canvases[canvas_name] = canvas;
            return canvas;
        }
    }
}
