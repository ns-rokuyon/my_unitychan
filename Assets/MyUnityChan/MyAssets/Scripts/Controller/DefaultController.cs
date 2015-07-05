using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DefaultController : PlayerController {
        void Update() {
            if ( PauseManager.isPausing() ) {
                if ( Input.GetKeyDown("space") ) {
                    // finish pause
                    PauseManager.Instance.pause(false);
                }
                return;
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
            inputs[(int)Movement.JUMP] = Input.GetKeyDown("space");
            inputs[(int)Movement.SLIDING] = Input.GetKeyDown("z");
            inputs[(int)Movement.ATTACK] = Input.GetKeyDown("x");
            inputs[(int)Movement.PROJECTILE] = Input.GetKeyDown("c");
            inputs[(int)Movement.DASH] = Input.GetKey("v");
            inputs[(int)Movement.TEST] = Input.GetKey("t");
        }

    }
}