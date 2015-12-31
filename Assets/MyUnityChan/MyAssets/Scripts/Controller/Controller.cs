using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class Controller : ObjectBase {
        public enum InputCode {
            JUMP = 0,
            SLIDING,
            ATTACK,
            PROJECTILE,
            DASH,
            PAUSE,
            TEST,
            CANCEL,
            DOWN,
            UP,
            LEFT,
            RIGHT,
            len
        };

        protected Character self;

        protected List<bool> inputs;
        protected float horizontal_input;
        protected float vertical_input;

        // Use this for initialization
        void Awake() {
            inputs = new List<bool>();
            for ( int i = 0; i < (int)InputCode.len; i++ ) {
                inputs.Add(false);
            }
            horizontal_input = 0.0f;
            vertical_input = 0.0f;
        }

        // Update is called once per frame
        void Update() {
        }

        public List<bool> getAllInputs() {
            return inputs;
        }

        public void setSelf(Character ch) {
            self = ch;
        }

        protected void clearAllInputs() {
            for ( int i = 0; i < inputs.Count; i++ ) {
                horizontal_input = 0.0f;
                vertical_input = 0.0f;
                inputs[i] = false;
            }
        }

        public CommandRecorder getCommandRecorder() {
            return GetComponent<CommandRecorder>();
        }

        public bool keyCancel() { return inputs[(int)InputCode.CANCEL]; }
        public bool keyJump() { return inputs[(int)InputCode.JUMP]; }
        public bool keySliding() { return inputs[(int)InputCode.SLIDING]; }
        public bool keyAttack() { return inputs[(int)InputCode.ATTACK]; }
        public bool keyProjectile() { return inputs[(int)InputCode.PROJECTILE]; }
        public bool keyDash() { return inputs[(int)InputCode.DASH]; }
        public bool keyPause() { return inputs[(int)InputCode.PAUSE]; }
        public bool keyTest() { return inputs[(int)InputCode.TEST]; }
        public bool keyUp() { return inputs[(int)InputCode.UP]; }
        public bool keyDown() { return inputs[(int)InputCode.DOWN]; }
        public bool keyLeft() { return inputs[(int)InputCode.LEFT]; }
        public bool keyRight() { return inputs[(int)InputCode.RIGHT]; }
        public float keyHorizontal() { return horizontal_input; }
        public float keyVertical() { return vertical_input; }

    }

    public abstract class AIController : Controller {
        public bool isStopped = false;
    }

}
