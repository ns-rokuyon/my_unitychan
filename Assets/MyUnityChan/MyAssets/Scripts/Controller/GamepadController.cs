﻿using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class GamepadController : PlayerController {
        protected override void watchInput() {
            inputs[(int)InputCode.JUMP] = Input.GetKeyDown("joystick button 0");        // A
            inputs[(int)InputCode.GUARD] = Input.GetKey("joystick button 1");       // B
            inputs[(int)InputCode.ATTACK] = Input.GetKeyDown("joystick button 3");      // Y
            inputs[(int)InputCode.PROJECTILE] = Input.GetKeyDown("c");      // TODO: assign button
            inputs[(int)InputCode.WEAPON] = Input.GetKeyDown("f");          // TODO: assign button
            inputs[(int)InputCode.DASH] = Input.GetKey("joystick button 4");            // L
            inputs[(int)InputCode.PAUSE] = Input.GetKeyDown("joystick button 7");       // Home
            inputs[(int)InputCode.TEST] = Input.GetKey("joystick button 2");            // X
            inputs[(int)InputCode.CANCEL] = Input.GetKey("joystick button 1");          // B
        }
    }
}
