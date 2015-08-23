using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AreaManager : SingletonObjectBase<AreaManager> {
        private Dictionary<string,Area> areas;

        void Awake() {
            areas = new Dictionary<string,Area>();

            Area[] found = FindObjectsOfType<Area>();
            foreach ( Area area in found ) {
                string name = area.gameObject.name;
                if ( areas.ContainsKey(name) ) {
                    Debug.LogError("duplicate area name = " + name);
                }
                areas[name] = area;
            }
        }

        // Use this for initialization
        void Start() {
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

        public Area getAreaFromMemberObject(GameObject obj) {
            foreach ( KeyValuePair<string, Area> pair in areas ) {
                if ( pair.Value.isIn(obj.transform.position) ) {
                    return pair.Value;
                }
            }
            return null;
        }

        public void registerAreaConnectionInfo(GameObject from, GameObject to) {
            Area area = getAreaFromMemberObject(from);
            area.addAreaConnections(from, to);
        }
    }
}