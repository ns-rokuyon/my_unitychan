﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class ShooterBase : ObjectBase {
        public bool on = false;
        public int shooting_frame;
        public int interval_frame;

        protected int start_frame;
        protected int frame_count;
        protected bool sleep;
        protected int sleep_start_frame;

        void Start() {
            baseStart();
        }

        void Update() {
            baseUpdate();
        }

        // Angle of shooting
        public virtual Vector3 angle() {
            return new Vector3(1.0f, 0.0f, 0.0f);
        }

        public virtual void shoot() { }


        public void baseStart() {
            sleep = false;
            frame_count = 0;
        }

        public void baseUpdate() {
            if ( !on ) return;
            if ( PauseManager.isPausing() ) return;

            if ( sleep ) {
                if ( Time.frameCount - sleep_start_frame >= interval_frame ) {
                    sleep = false;
                    return;
                }
                return;
            }

            if ( frame_count > 0 ) {
                if ( shooting_frame <= frame_count ) {
                    frame_count = 0;
                    sleep = true;
                    sleep_start_frame = Time.frameCount;
                    return;
                }
                shoot();
                frame_count++;
            }
            else {
                shoot();
                frame_count = 1;
            }
        }
    }
}
