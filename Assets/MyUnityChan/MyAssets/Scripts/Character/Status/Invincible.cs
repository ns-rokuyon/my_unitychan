using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Invincible : ObjectBase {
        public bool all_time = false;

        private bool is_invincible;

        // Use this for initialization
        void Start() {
            is_invincible = false;
        }

        public bool now() {
            if ( all_time )
                return true;
            return is_invincible;
        }

        public void enable(int frame) {
            is_invincible = true;
            if ( all_time )
                return;
            Observable.TimerFrame(frame)
                .Subscribe(_ => is_invincible = false);
        }

    }
}
