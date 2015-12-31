using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class PlayerController : Controller {
        protected abstract void watchInput();

        void Update() {
            if ( self.isInputLocked() && !PauseManager.isPausing() ) {
                // ignore any inputs
                clearAllInputs();
                return;
            }

            if ( self.isFrozen() ) {
                // ignore any inputs
                clearAllInputs();
                return;
            }

            watchInput();
            updateDirectionKey();

            if ( PauseManager.isPausing() ) {
                PauseManager.controlOnPause();
                return;
            }
        }

        protected void updateDirectionKey() {
            inputs[(int)InputCode.RIGHT] = false;
            inputs[(int)InputCode.LEFT] = false;
            inputs[(int)InputCode.UP] = false;
            inputs[(int)InputCode.DOWN] = false;

            if ( horizontal_input > 0 ) {
                inputs[(int)InputCode.RIGHT] = true;
                inputs[(int)InputCode.LEFT] = false;
            }
            if ( horizontal_input < 0 ) {
                inputs[(int)InputCode.RIGHT] = false;
                inputs[(int)InputCode.LEFT] = true;
            }
            if ( vertical_input > 0 ) {
                inputs[(int)InputCode.UP] = true;
                inputs[(int)InputCode.DOWN] = false;
            }
            if ( vertical_input < 0 ) {
                inputs[(int)InputCode.UP] = false;
                inputs[(int)InputCode.DOWN] = true;
            }

        }

    }
}