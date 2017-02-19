using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class GUIObjectBase : ObjectBase {
        private Canvas _parent_canvas;
        public Canvas parent_canvas {
            get {
                return _parent_canvas ?? (_parent_canvas = transform.parent.GetComponent<Canvas>());
            }
        }

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
