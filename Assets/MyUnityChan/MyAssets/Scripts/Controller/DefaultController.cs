﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DefaultController : PlayerController {
        void Update() {
            if ( self.isInputLocked() ) {
                // ignore any inputs
                clearAllInputs();
                return;
            }

            horizontal_input = Input.GetAxis("Horizontal");
            inputs[(int)Movement.JUMP] = Input.GetKeyDown("space");
            inputs[(int)Movement.SLIDING] = Input.GetKeyDown("z");
            inputs[(int)Movement.ATTACK] = Input.GetKeyDown("x");
            inputs[(int)Movement.PROJECTILE] = Input.GetKeyDown("c");
            inputs[(int)Movement.DASH] = Input.GetKey("v");
        }

    }
}