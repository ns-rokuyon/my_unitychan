using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FrameTimer : Timer {
        public int start_frame = 0;
        public int count = 0;
        public int duration = 0;
        public bool endless = false;

        public void setTimer(int time) {
            start_frame = Time.frameCount;
            duration = time;
            running = true;
            if ( time == 0 ) {
                endless = true;
            }
        }

        public int now() {
            return count;
        }

        // Use this for initialization
        void Start() {
            running = true;
        }

        // Update is called once per frame
        void Update() {
            if ( PauseManager.isPausing() ) {
                return;
            }
            if ( running ) {
                count++;
                if ( !endless && count >= duration ) {
                    running = false;
                }
            }
        }

        public override void initialize() {
            start_frame = 0;
            count = 0;
            duration = 0;
            endless = false;
        }

        public override void finalize() {
            running = false;
            endless = false;
        }
    }

    public class FrameTimerState : TimerState {

        public override void createTimer(int time) {
            FrameTimer timer = TimerManager.self().create<FrameTimer>(Const.Prefab.Timer["FRAME_TIMER"], true);
            timer.gameObject.setParent(Hierarchy.Layout.TIMER);
            timer.setTimer(time);

            int id = TimerManager.self().add(timer);

            timer_id = id;
        }

        public override void createTimer(float time) {
            createTimer((int)time);
        }

    }
}
