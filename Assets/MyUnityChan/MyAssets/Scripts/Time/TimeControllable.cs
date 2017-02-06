﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class TimeControllable : ObjectBase {
        public virtual bool paused {
            get {
                return Time.timeScale == 0.0f;
            }
        }

        public virtual float deltaTime {
            get {
                return Time.deltaTime;
            }
        }
    }
}