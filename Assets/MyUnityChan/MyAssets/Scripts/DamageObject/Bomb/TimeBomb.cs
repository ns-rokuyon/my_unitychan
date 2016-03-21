using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class TimeBomb : Bomb {
        public float time;

        void Start() {
            initialize();
        }

        protected IEnumerator startCountDown() {
            yield return new WaitForSeconds(time);
            explode();
        }

        public override void initialize() {
            StartCoroutine("startCountDown");
        }

        public override void finalize() {
        }

    }
}
