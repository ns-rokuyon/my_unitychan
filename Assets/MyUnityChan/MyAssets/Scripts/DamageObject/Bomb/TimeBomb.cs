using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class TimeBomb : Bomb {
        public int frame;

        void Start() {
            initialize();
        }

        public override void initialize() {
            delay(frame, () => explode());
        }

        public override void finalize() {
        }
    }
}
