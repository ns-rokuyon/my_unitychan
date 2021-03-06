﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class AreaManager : SingletonObjectBase<AreaManager> {
        public bool debug;

        public Dictionary<string, Area> areas { get; private set; }
        public string now_area_name { get; protected set; }

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

            if ( debug ) {
                foreach ( var item in areas ) {
                    DebugManager.log("" + item.Key + ": " + item.Value.gameObject.transform.position);
                }
            }
        }

        public static int getAreaNum() {
            return self().areas.Count;
        }

        public static int getPassedAreaNum() {
            return self().areas.Count(kv => kv.Value.passed);
        }

        public Area getArea(string name) {
            return areas[name];
        }

        public Area getNowArea() {
            return getArea(now_area_name);
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

        public void relabelObject(GameObject obj) {
            foreach ( KeyValuePair<string, Area> pair in areas ) {
                if ( pair.Value.isIn(obj.transform.position) ) {
                    pair.Value.relabel(obj, now_area_name);
                }
            }
        }

        public void registerAreaConnectionInfo(GameObject from, GameObject to) {
            Area area = getAreaFromMemberObject(from);
            area.addAreaConnections(from, to);

            Area to_area = getAreaFromMemberObject(to);
            area.addConnectedArea(to_area);
        }

        public void reportPlayerEntered(Area area) {
            now_area_name = area.gameObject.name;
        }

        public void manageGameObjectsInArea(Area now_area, List<Area> connected_area) {
            foreach ( KeyValuePair<string, Area> pair in areas ) {
                Area area = pair.Value;
                if ( connected_area.IndexOf(area) == -1 ) {
                    // For not connected areas
                    area.deactivateGameObjects();
                }
                else {
                    // For connected areas
                    area.activateGameObjects();
                }
            }
            now_area.activateGameObjects();
        }
    }
}