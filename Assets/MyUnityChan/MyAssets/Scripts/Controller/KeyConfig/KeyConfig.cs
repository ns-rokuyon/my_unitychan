using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class KeyConfig : ObjectBase {

        public static readonly Controller.InputCode[] fixed_codes = {
            Controller.InputCode.UP, Controller.InputCode.DOWN,
            Controller.InputCode.LEFT, Controller.InputCode.RIGHT,
            Controller.InputCode.HORIZONTAL, Controller.InputCode.VERTICAL
        };

        public AxisSlot horizontal_axis { get; protected set; }
        public AxisSlot vertical_axis { get; protected set; }

        void Start() {
            horizontal_axis = new AxisSlot(Controller.InputCode.HORIZONTAL, "Horizontal");
            vertical_axis = new AxisSlot(Controller.InputCode.VERTICAL, "Vertical");

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

        [System.Serializable]
        public abstract class InputSource<T> {
            [SerializeField, ReadOnly]
            public Controller.InputCode code;

            [SerializeField]
            public bool configurable = true;

            public InputSource(Controller.InputCode c) {
                code = c;
            }

            public abstract string symbol { get; }
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

        [System.Serializable]
        public class AxisSlot : StickSlot {
            public string axis_name { get; private set; }

            public AxisSlot(Controller.InputCode code, string _axis_name) : base(code) {
                axis_name = _axis_name;
                configurable = false;
            }

            public override string symbol {
                get {
                    return code.ToString();
                }
            }

            public override float read() {
                if ( Time.timeScale > 0.0f ) {
                    return Input.GetAxis(axis_name);
                }
                else {
                    return Input.GetAxisRaw(axis_name);
                }
            }
        }
    }
}