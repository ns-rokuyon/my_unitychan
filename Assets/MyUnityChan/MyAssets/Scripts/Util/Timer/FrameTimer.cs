using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FrameTimer : Timer {
        private int start_frame = 0;
        private int count = 0;
        private int duration = 0;

        public void setTimer(int time) {
            start_frame = Time.frameCount;
            duration = time;
            running = true;
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
            if ( running ) {
                count++;
                if ( count >= duration ) {
                    running = false;
                }
            }
        }
    }

    public class FrameTimerState : TimerState {

        public override void createTimer(int time) {
            GameObject timer_object = GameObject.Instantiate(TimerManager.self().frame_timer_prefab) as GameObject;
            timer_object.setParent(Hierarchy.Layout.TIMER);
            FrameTimer timer = timer_object.GetComponent<FrameTimer>();
            timer.setTimer(time);

            int id = timer_object.GetInstanceID();
            TimerManager.self().add(id, timer);

            timer_id = id;
        }

        public override void createTimer(float time) {
            createTimer((int)time);
        }

    }
}
