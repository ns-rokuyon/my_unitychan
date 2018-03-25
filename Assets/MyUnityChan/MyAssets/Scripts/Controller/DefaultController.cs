using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;
using System.Linq;

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

            foreach ( var code in keyconfig.codes ) {
                inputs[(int)code] = keyconfig.read(code);
            }
        }
    }
}