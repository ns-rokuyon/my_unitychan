#define USE_CHRONOS
using UnityEngine;
using UnityChan;
using System.Linq;
using System.Collections;

#if USE_CHRONOS
using Chronos;
#endif

namespace MyUnityChan {
    public class PauseManager : SingletonObjectBase<PauseManager> {
        public delegate void PauseOffCallBack();
        public delegate void PauseControlMethod();

        private PauseOffCallBack pause_off_callback;
        public PauseControlMethod control_on_pause;

        public int delay_control_count = 2;
        public bool paused = false;

        public void pause(bool on, PauseControlMethod control=null, PauseOffCallBack callback=null) {
            if ( on ) {
                paused = on;

                changeTimeScaleOnPause();

                pause_off_callback = callback;
                control_on_pause = control;

                GUIObjectBase.getCanvas("Canvas_HUD").GetComponent<Canvas>().enabled = false;
                GameStateManager.self().player_manager.getNowPlayer()
                    .GetComponent<SpringManager>().enabled = false;
            }
            else {
                GameStateManager.self().player_manager.getNowPlayer()
                    .GetComponent<SpringManager>().enabled = true;

                changeTimeScaleOnResume();

                if ( pause_off_callback != null ) {
                    pause_off_callback();
                }
                GUIObjectBase.getCanvas("Canvas_HUD").GetComponent<Canvas>().enabled = true;
                pause_off_callback = null; 
                control_on_pause = null;
                delay_control_count = 2;
                paused = false;
            }
        }

        private void changeTimeScaleOnPause() {
#if USE_CHRONOS
            Const.TimeScaleChronosClockNames.ForEach(key => {
                Timekeeper.instance.Clock(key).localTimeScale = 0.0f;
            });
#else
            Time.timeScale = 0.0f;
#endif
        }

        private void changeTimeScaleOnResume() {
#if USE_CHRONOS
            Const.TimeScaleChronosClockNames.ForEach(key => {
                Timekeeper.instance.Clock(key).localTimeScale = 1.0f;
            });
#else
            Time.timeScale = 1.0f;
#endif
        }

        public static void setPauseControlMethod(PauseControlMethod control) {
            self().control_on_pause = control;
        }

        public int getDelayCount() {
            return delay_control_count;
        }

        public void countdownDelay() {
            delay_control_count--;
            if ( delay_control_count < 0 ) {
                delay_control_count = 0;
            }
        }

        public static void controlOnPause() {
            if ( Instance.getDelayCount() == 0 && Instance.control_on_pause != null ) {
                Instance.control_on_pause();
            }
            Instance.countdownDelay();
        }

        public static bool isPausing() {
            return Instance.paused;
            //if ( Time.timeScale > 0 ) {
            //    return false;
            //}
            //return true;
        }
    }

    public class PauseState : StructBase {

    }

    public class RigidbodyPauseState : PauseState {
        public Vector3 velocity { get; set; }
        public Vector3 angular_velocity { get; set; }

        public RigidbodyPauseState(Rigidbody rb) : base() {
            set(rb);
        }

        public void set(Rigidbody rb) {
            if ( rb ) {
                velocity = rb.velocity;
                angular_velocity = rb.angularVelocity;
            }
            else {
                velocity = Vector3.zero;
                angular_velocity = Vector3.zero;
            }
        }

        public void pause(Rigidbody rb) {
            set(rb);
        }
    }
}
