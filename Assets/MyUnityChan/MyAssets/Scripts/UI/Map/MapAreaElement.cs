using UnityEngine;
using System.Collections.Generic;
using Vectrosity;

namespace MyUnityChan {
    public class MapAreaElement : MapElement {
        public GameObject area_object;
        public Area area_ref { get; protected set; }
        public PassMonitoring pass_monitoring { get; private set; }

        private List<Tuple<GameObject, GameObject>> point_sets;

        void Awake() {
            point_sets = new List<Tuple<GameObject, GameObject>>();
            pass_monitoring = new PassMonitoring();
        }

        void Start() {
            area_ref = area_object.GetComponent<Area>();
        }

        // Update is called once per frame
        void Update() {
            if ( pass_monitoring != null ) {
                pass_monitoring.monitor(this);
            }

        }

        public void setAreaRef(Area obj) {
            area_ref = obj;
        }

        public Area getAreaRef() {
            return area_ref;
        }

        public void addPointSet(GameObject pointA, GameObject pointB, GameObject map) {
            point_sets.Add(new Tuple<GameObject, GameObject>(pointA, pointB));
            foreach ( Tuple<GameObject,GameObject> point_set in point_sets ) {
                List<Vector3> points = new List<Vector3>();
                points.Add(point_set._1.transform.position);
                points.Add(point_set._2.transform.position);
                VectorLine line = new Vectrosity.VectorLine("MapView/line", points, 1.0f);
                line.SetCanvas(map.GetComponent<Canvas>());
                line.Draw();
            }
        }

        public class PassMonitoring {
            public void monitor(MapAreaElement self) {
                Area area = self.getAreaRef();
                if ( area && area.isPassed() ) {
                    self.gameObject.GetComponent<MeshRenderer>().material.color =
                        new Color32(0, 255, 0, 60);
                    self.pass_monitoring = null;
                }
            }

        }
    }
}
