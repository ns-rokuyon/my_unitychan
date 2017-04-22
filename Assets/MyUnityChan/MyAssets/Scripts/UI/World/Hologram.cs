using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Hologram : ObjectBase {
        public SensorZone zone { get; set; }

        public virtual void appear() { }
        public virtual void disappear() { }

        public virtual void Start() {
            if ( zone ) {
                zone.onPlayerEnterCallback = appear;
                zone.onPlayerExitCallback = disappear;
            }
        }
    }
}