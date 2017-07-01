using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class CSFHIHologram : Hologram {
        public InterfaceAnimManager manager { get; protected set; }

        public void Awake() {
            manager = GetComponent<InterfaceAnimManager>();
        }

        public override void appear(Player player = null, Collider collider = null) {
            manager.startAppear();
        }

        public override void disappear(Player player = null, Collider collider = null) {
            manager.startDisappear();
        }
    }
}