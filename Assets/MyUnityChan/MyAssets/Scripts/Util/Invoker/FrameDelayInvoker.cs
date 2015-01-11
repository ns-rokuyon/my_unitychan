using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FrameDelayInvoker : Invoker {
        protected FrameTimerState timer;

        public delegate void DelayDelegate();
        private DelayDelegate delay_func;

        void Awake() {
            timer = new FrameTimerState();
        }

        public void set(int frame, DelayDelegate func) {
            delay_func = func;
            timer.createTimer(frame);
        }

        protected override void invoke() {
            delay_func();
        }

        protected override bool schedule() {
            return timer.isFinished();
        }
    }
}