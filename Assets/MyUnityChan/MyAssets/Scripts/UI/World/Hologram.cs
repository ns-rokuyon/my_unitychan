using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Hologram : ObjectBase {
        public SensorZone zone { get; set; }

        public virtual void appear(Player player = null, Collider collider = null) { }
        public virtual void disappear(Player player = null, Collider collider = null) { }

        public virtual void Start() {
            if ( zone ) {
                zone.onPlayerEnterCallback = appear;
                zone.onPlayerExitCallback = disappear;
            }
        }
    }
}