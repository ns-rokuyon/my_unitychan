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

            if ( PauseManager.isPausing() ) {
                PauseManager.controlOnPause();
                return;
            }
        }
    }
}