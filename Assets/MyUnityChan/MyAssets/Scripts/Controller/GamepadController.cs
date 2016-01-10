using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class GamepadController : PlayerController {
        protected override void watchInput() {
            if ( Time.timeScale > 0.0f ) {
                horizontal_input = Input.GetAxis("Horizontal");
                vertical_input = Input.GetAxis("Vertical");
            }
            else {
                horizontal_input = Input.GetAxisRaw("Horizontal");
                vertical_input = Input.GetAxisRaw("Vertical");
            }
            inputs[(int)InputCode.JUMP] = Input.GetKeyDown("joystick button 0");        // A
            inputs[(int)InputCode.GUARD] = Input.GetKey("joystick button 1");       // B
            inputs[(int)InputCode.ATTACK] = Input.GetKeyDown("joystick button 3");      // Y
            inputs[(int)InputCode.PROJECTILE] = Input.GetKeyDown("c");
            inputs[(int)InputCode.DASH] = Input.GetKey("joystick button 4");            // L
            inputs[(int)InputCode.PAUSE] = Input.GetKeyDown("joystick button 7");       // Home
            inputs[(int)InputCode.TEST] = Input.GetKey("joystick button 2");            // X
            inputs[(int)InputCode.CANCEL] = Input.GetKey("joystick button 1");          // B
        }
    }
}
