using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DefaultController : PlayerController {
        void Update() {
            if ( PauseManager.isPausing() ) {
                if ( Input.GetKeyDown("space") ) {
                    // finish pause
                    PauseManager.Instance.pause(false);
                    return;
                }
            }

            if ( self.isInputLocked() ) {
                // ignore any inputs
                clearAllInputs();
                return;
            }

            if ( self.isFrozen() ) {
                // ignore any inputs
                clearAllInputs();
                return;
            }

            horizontal_input = Input.GetAxis("Horizontal");
            vertical_input = Input.GetAxis("Vertical");
            inputs[(int)InputCode.JUMP] = Input.GetKeyDown("space");
            inputs[(int)InputCode.SLIDING] = Input.GetKeyDown("z");
            inputs[(int)InputCode.ATTACK] = Input.GetKeyDown("x");
            inputs[(int)InputCode.PROJECTILE] = Input.GetKeyDown("c");
            inputs[(int)InputCode.DASH] = Input.GetKey("v");
            inputs[(int)InputCode.PAUSE] = Input.GetKeyDown("p");
            inputs[(int)InputCode.TEST] = Input.GetKey("t");
        }

    }
}