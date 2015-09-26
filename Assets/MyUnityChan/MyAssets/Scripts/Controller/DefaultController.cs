using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DefaultController : PlayerController {
        /*
         * Keyboard Controller (Default)
         */

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

            if ( Time.timeScale > 0.0f ) {
                horizontal_input = Input.GetAxis("Horizontal");
                vertical_input = Input.GetAxis("Vertical");
            }
            else {
                horizontal_input = Input.GetAxisRaw("Horizontal");
                vertical_input = Input.GetAxisRaw("Vertical");
            }
            inputs[(int)InputCode.JUMP] = Input.GetKeyDown("space");
            inputs[(int)InputCode.SLIDING] = Input.GetKeyDown("z");
            inputs[(int)InputCode.ATTACK] = Input.GetKeyDown("x");
            inputs[(int)InputCode.PROJECTILE] = Input.GetKeyDown("c");
            inputs[(int)InputCode.DASH] = Input.GetKey("v");
            inputs[(int)InputCode.PAUSE] = Input.GetKeyDown("p");
            inputs[(int)InputCode.TEST] = Input.GetKey("t");
            inputs[(int)InputCode.CANCEL] = Input.GetKey("b");

            if ( PauseManager.isPausing() ) {
                PauseManager.controlOnPause();
                return;
            }
        }

    }
}