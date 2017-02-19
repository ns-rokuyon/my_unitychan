using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MapBuilder : SingletonObjectBase<MapBuilder> {
        public string build_to = "MapViewer/Map";
        public Vector3 scale = new Vector3(0.01f, 0.01f, 0.01f);

        public List<MapElement> elements { get; protected set; }
        public Dictionary<string, MapAreaElement> area_elements { get; protected set; }

        void Awake() {
            elements = new List<MapElement>(GetComponentsInChildren<MapElement>());
            area_elements = new Dictionary<string, MapAreaElement>();
        }

        void Start() {
            elements.Where(e => e is MapAreaElement).ToList().ForEach(e => {
                string _name = (e as MapAreaElement).area.name;
                area_elements.Add(_name, e as MapAreaElement);
            });
        }

        public LayerMask layer() {
            return LayerMask.NameToLayer("MapElement");
        }

        public MapAreaElement findAreaElementByAreaName(string name) {
            if ( !area_elements.ContainsKey(name) ) {
                return null;
            }
            return area_elements[name];
        }
    }
}