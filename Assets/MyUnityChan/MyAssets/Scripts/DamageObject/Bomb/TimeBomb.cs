using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class TimeBomb : Bomb {
        public int frame;

        void Start() {
            initialize();
        }

        public override Vector3 getInitPosition(Transform owner) {
            return owner.position.add(owner.forward.x * 0.5f, 0.2f, 0);
        }

        public override void initialize() {
            delay(frame, () => explode());
        }

        public override void finalize() {
        }
    }
}
