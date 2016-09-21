using UnityEngine;

namespace MyUnityChan {
    public class MapBuilder : SingletonObjectBase<MapBuilder> {
        public string build_to = "MapViewer/Map";
        public Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);

        public LayerMask layer() {
            return LayerMask.NameToLayer("MapElement");
        }
    }
}