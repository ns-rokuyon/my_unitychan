﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Effect : EffectBase {
        FrameTimerState timer = null;

        // Update is called once per frame
        void Update() {
            if ( timer != null && timer.isFinished() ) {
                ObjectPoolManager.releaseGameObject(gameObject, resource_path);
            }
        }

        public void ready(Vector3 pos, int frame, string _resource_path) {
            resource_path = _resource_path;
            transform.position = pos;

            initialize();
            timer.createTimer(frame);
        }

        public override void initialize() {
            timer = new FrameTimerState();
        }
        public override void finalize() {
            timer = null;
        }
    }
}