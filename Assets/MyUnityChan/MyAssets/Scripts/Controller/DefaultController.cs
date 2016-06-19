using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class DefaultController : PlayerController {
        /*
         * Keyboard Controller (Default)
         */

        protected override void watchInput() {
            if ( Time.timeScale > 0.0f ) {
                horizontal_input = Input.GetAxis("Horizontal");
                vertical_input = Input.GetAxis("Vertical");
            }
            else {
                horizontal_input = Input.GetAxisRaw("Horizontal");
                vertical_input = Input.GetAxisRaw("Vertical");
            }
            inputs[(int)InputCode.JUMP] = Input.GetKeyDown("space");
            inputs[(int)InputCode.ATTACK] = Input.GetKeyDown("x");
            inputs[(int)InputCode.PROJECTILE] = Input.GetKey("c");
            inputs[(int)InputCode.WEAPON] = Input.GetKey("f");
            inputs[(int)InputCode.DASH] = Input.GetKey("v");
            inputs[(int)InputCode.GUARD] = Input.GetKey("z");
            inputs[(int)InputCode.SWITCH_BEAM] = Input.GetKeyDown("g");
            inputs[(int)InputCode.PAUSE] = Input.GetKeyDown("p");
            inputs[(int)InputCode.TEST] = Input.GetKeyDown("t");
            inputs[(int)InputCode.CANCEL] = Input.GetKey("b");
            inputs[(int)InputCode.PREV_TAB] = Input.GetKeyDown("z");
            inputs[(int)InputCode.NEXT_TAB] = Input.GetKeyDown("n");
        }

    }
}