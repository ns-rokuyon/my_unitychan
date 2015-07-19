using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AreaManager : SingletonObjectBase<AreaManager> {
        private Dictionary<string,Area> areas;

        void Awake() {
            areas = new Dictionary<string,Area>();
        }

        // Use this for initialization
        void Start() {
            Area[] found = FindObjectsOfType<Area>();
            foreach ( Area area in found ) {
                string name = area.gameObject.name;
                if ( areas.ContainsKey(name) ) {
                    Debug.LogError("duplicate area name = " + name);
                }
                areas[name] = area;
            }
        }

        // Update is called once per frame
        void Update() {
            
        }

        public Area getArea(string name) {
            return areas[name];
        }

        public string getAreaNameFromObject(GameObject obj) {
            foreach ( KeyValuePair<string, Area> pair in areas ) {
                if ( pair.Value.isIn(obj.transform.position) ) {
                    return pair.Key;
                }
            }
            return null;
        }

        public bool isInArea(GameObject obj, string name) {
            return areas[name].isIn(obj.transform.position);
        }
    }
}