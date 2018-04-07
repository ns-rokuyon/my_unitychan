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
            foreach ( var code in keyconfig.codes ) {
                inputs[(int)code] = keyconfig.read(code);
            }
        }
    }
}