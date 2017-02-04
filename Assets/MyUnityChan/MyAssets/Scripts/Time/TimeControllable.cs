using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class TimeControllable : ObjectBase {
        public bool paused { get; protected set; }

        public virtual float deltaTime {
            get {
                return Time.deltaTime;
            }
        }
    }
}
