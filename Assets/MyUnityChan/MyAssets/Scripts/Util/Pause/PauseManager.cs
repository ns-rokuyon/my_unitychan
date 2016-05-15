using UnityEngine;
using UnityChan;
using System.Collections;

namespace MyUnityChan {
    public class PauseManager : SingletonObjectBase<PauseManager> {
        public delegate void PauseOffCallBack();
        public delegate void PauseControlMethod();

        private PauseOffCallBack pause_off_callback;
        public PauseControlMethod control_on_pause;

        public int delay_control_count = 2;

        public void pause(bool on, PauseControlMethod control=null, PauseOffCallBack callback=null) {
            if ( on ) {
                Time.timeScale = 0;
                pause_off_callback = callback;
                control_on_pause = control;
                GUIObjectBase.getCanvas("Canvas").GetComponent<Canvas>().enabled = false;
                GameStateManager.self().player_manager.getNowPlayer()
                    .GetComponent<SpringManager>().enabled = false;
            }
            else {
                GameStateManager.self().player_manager.getNowPlayer()
                    .GetComponent<SpringManager>().enabled = true;
                Time.timeScale = 1.0f;
                if ( pause_off_callback != null ) {
                    pause_off_callback();
                }
                GUIObjectBase.getCanvas("Canvas").GetComponent<Canvas>().enabled = true;
                pause_off_callback = null; 
                control_on_pause = null;
                delay_control_count = 2;
            }
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
            if ( Time.timeScale > 0 ) {
                return false;
            }
            return true;
        }
    }
}
