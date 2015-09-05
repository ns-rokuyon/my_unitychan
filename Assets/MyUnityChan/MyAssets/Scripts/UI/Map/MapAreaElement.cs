using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MapAreaElement : MapElement {
        public Area area_ref;

        public PassMonitoring pass_monitoring;

        void Awake() {
            area_ref = null;
            pass_monitoring = null;
        }

        // Use this for initialization
        void Start() {
            pass_monitoring = new PassMonitoring();
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

        public class PassMonitoring {
            public void monitor(MapAreaElement self) {
                if ( self.getAreaRef().isPassed() ) {
                    self.gameObject.GetComponent<MeshRenderer>().material.color = 
                        new Color(0, 255, 0, self.gameObject.GetComponent<MeshRenderer>().material.color.a);
                    self.pass_monitoring = null;
                }
            }

        }
    }
}
