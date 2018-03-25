using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class KeyConfig : ObjectBase {

        void Start() {
            setDefault();
        }

        // Set default buttons and sticks
        public abstract void setDefault();

        // Support input code list
        public abstract IEnumerable<Controller.InputCode> codes { get; }

        // Read input of code
        public abstract bool read(Controller.InputCode code);

        // Get symbol to display of code
        public abstract string symbol(Controller.InputCode code);

        // Initialize all slots
        public void initSlots() {
            var button_codes = new Controller.InputCode[] {
                Controller.InputCode.ATTACK, Controller.InputCode.CANCEL, Controller.InputCode.DASH,
                Controller.InputCode.GRAPPLE, Controller.InputCode.GUARD, Controller.InputCode.JUMP,
                Controller.InputCode.NEXT_TAB, Controller.InputCode.PAUSE, Controller.InputCode.PREV_TAB,
                Controller.InputCode.PROJECTILE, Controller.InputCode.SPECIAL_01, Controller.InputCode.SPECIAL_02,
                Controller.InputCode.SPECIAL_03, Controller.InputCode.SPECIAL_04, Controller.InputCode.SPECIAL_05,
                Controller.InputCode.SWITCH_BEAM, Controller.InputCode.TEST, Controller.InputCode.TRANSFORM,
                Controller.InputCode.WEAPON
            };

            var stick_codes = new Controller.InputCode[] {
                Controller.InputCode.LEFT, Controller.InputCode.RIGHT,
                Controller.InputCode.UP, Controller.InputCode.DOWN
            };
        }

        [System.Serializable]
        public abstract class InputSource<T> {
            [SerializeField, ReadOnly]
            public Controller.InputCode code;

            public InputSource(Controller.InputCode c) {
                code = c;
            }

            public abstract T read();
        }

        [System.Serializable]
        public abstract class ButtonSlot : InputSource<bool> {
            public ButtonSlot(Controller.InputCode c) : base(c) {
            }
        }

        [System.Serializable]
        public abstract class StickSlot : InputSource<float> {
            public StickSlot(Controller.InputCode c) : base(c) {
            }
        }
    }
}