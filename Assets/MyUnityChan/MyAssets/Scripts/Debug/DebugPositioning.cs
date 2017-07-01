using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class DebugPositioning : ObjectBase {
        public List<Vector3> positions;
        public int index;
        public bool on;

        void Start() {
            if ( !on )
                return;
            if ( positions.Count == 0 )
                return;
            if ( index < 0 || index > positions.Count )
                return;
            gameObject.transform.position = positions[index];
            DebugManager.log("Move position", this.gameObject, Const.Loglevel.DEBUG);
        }
    }
}