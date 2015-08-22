using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Invincible : ObjectBase {
        public bool all_time = false;

        private TimerState timer;
        private bool is_invincible;

        // Use this for initialization
        void Start() {
            timer = new FrameTimerState();
            is_invincible = false;
        }

        // Update is called once per frame
        void Update() {
            if ( all_time ) {
                is_invincible = true;
                return;
            }

            if ( timer != null && timer.isRunning() ) {
                is_invincible = true;
            }
            else {
                is_invincible = false;
            }
        }

        public bool now() {
            return is_invincible;
        }

        public void enable(int frame) {
            timer.createTimer(frame);
        }

    }
}
