using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public abstract class Controller : ObjectBase {
        public enum InputCode {
            JUMP = 0,
            ATTACK,
            PROJECTILE,
            WEAPON,
            DASH,
            GUARD,
            SWITCH_BEAM,
            GRAPPLE,
            PAUSE,
            TRANSFORM,
            TEST,
            CANCEL,
            DOWN,
            UP,
            LEFT,
            RIGHT,
            PREV_TAB,
            NEXT_TAB,
            SPECIAL_01,
            SPECIAL_02,
            SPECIAL_03,
            SPECIAL_04,
            SPECIAL_05,
            HORIZONTAL,
            VERTICAL,
            ANY_AXIS,
            len
        };

        protected Character self;

        protected List<bool> inputs;
        protected List<bool> raw_inputs;
        protected float horizontal_input;
        protected float raw_horizontal_input;
        protected float vertical_input;
        protected float raw_vertical_input;

        public UniRx.IObservable<float> keyStreamHorizontal { get; private set; }
        public UniRx.IObservable<float> keyStreamVertical { get; private set; }

        public bool show_debug_window;
        public GameObject debug_window { get; private set; }
        public Text directional_debug_text { get; private set; }
        public Text buttons_debug_text { get; private set; }

        // Use this for initialization
        public virtual void Awake() {
            inputs = new List<bool>();
            raw_inputs = new List<bool>();
            for ( int i = 0; i < (int)InputCode.len; i++ ) {
                inputs.Add(false);
                raw_inputs.Add(false);
            }
            horizontal_input = 0.0f;
            raw_horizontal_input = 0.0f;
            vertical_input = 0.0f;
            raw_vertical_input = 0.0f;

            keyStreamHorizontal = this.UpdateAsObservable().Select(_ => horizontal_input);
            keyStreamVertical = this.UpdateAsObservable().Select(_ => vertical_input);
        }

        public virtual void Start() {
            this.UpdateAsObservable()
                .Where(_ => show_debug_window)
                .Subscribe(_ => {
                    if ( !debug_window ) {
                        debug_window = PrefabInstantiater.createWorldUI("Prefabs/UI/World/ControllerData");
                        directional_debug_text = debug_window.transform.Find("Directional").gameObject.GetComponent<Text>();
                        buttons_debug_text = debug_window.transform.Find("Buttons").gameObject.GetComponent<Text>();
                    }
                    directional_debug_text.text = getStringDirectionalInputs();
                    buttons_debug_text.text = getStringButtonInputs();
                    debug_window.transform.position = transform.position.add(0, 1.0f, -1.0f);
                });

            this.ObserveEveryValueChanged(_ => show_debug_window)
                .Where(f => !f)
                .Subscribe(_ => {
                    if ( debug_window ) {
                        Destroy(debug_window);
                    }
                });
        }

        public List<bool> getAllInputs() {
            return inputs;
        }

        public void setSelf(Character ch) {
            self = ch;
        }

        public void clearAllInputs() {
            for ( int i = 0; i < inputs.Count; i++ ) {
                raw_inputs[i] = inputs[i];  // Copy inputs to raw_inputs before clearing
                inputs[i] = false;
            }
            raw_horizontal_input = horizontal_input;
            raw_vertical_input = vertical_input;
            horizontal_input = 0.0f;
            vertical_input = 0.0f;
        }

        public CommandRecorder getCommandRecorder() {
            return GetComponent<CommandRecorder>();
        }

        public bool getRawInput(InputCode code) {
            int c = (int)code;
            return raw_inputs[c] || inputs[c];
        }

        public float getRawHorizontalInput() {
            return raw_horizontal_input;
        }

        public float getRawVerticalInput() {
            return raw_vertical_input;
        }

        public bool keyCancel() { return inputs[(int)InputCode.CANCEL]; }
        public bool keyJump() { return inputs[(int)InputCode.JUMP]; }
        public bool keyAttack() { return inputs[(int)InputCode.ATTACK]; }
        public bool keyProjectile() { return inputs[(int)InputCode.PROJECTILE]; }
        public bool keyWeapon() { return inputs[(int)InputCode.WEAPON]; }
        public bool keyDash() { return inputs[(int)InputCode.DASH]; }
        public bool keyPause() { return inputs[(int)InputCode.PAUSE]; }
        public bool keyGuard() { return inputs[(int)InputCode.GUARD]; }
        public bool keySwitchBeam() { return inputs[(int)InputCode.SWITCH_BEAM]; }
        public bool keyGrapple() { return inputs[(int)InputCode.GRAPPLE]; }
        public bool keyTest() { return inputs[(int)InputCode.TEST]; }
        public bool keyTransform() { return inputs[(int)InputCode.TRANSFORM]; }
        public bool keyUp() { return inputs[(int)InputCode.UP]; }
        public bool keyDown() { return inputs[(int)InputCode.DOWN]; }
        public bool keyLeft() { return inputs[(int)InputCode.LEFT]; }
        public bool keyRight() { return inputs[(int)InputCode.RIGHT]; }
        public bool keyPrevTab() { return inputs[(int)InputCode.PREV_TAB]; }
        public bool keyNextTab() { return inputs[(int)InputCode.NEXT_TAB]; }
        public bool keySpecial01() { return inputs[(int)InputCode.SPECIAL_01]; }
        public bool keySpecial02() { return inputs[(int)InputCode.SPECIAL_02]; }
        public bool keySpecial03() { return inputs[(int)InputCode.SPECIAL_03]; }
        public bool keySpecial04() { return inputs[(int)InputCode.SPECIAL_04]; }
        public bool keySpecial05() { return inputs[(int)InputCode.SPECIAL_05]; }
        public float keyHorizontal() { return horizontal_input; }
        public float keyVertical() { return vertical_input; }

        public void inputKey(InputCode code, int frame = 1) {
            inputs[(int)code] = true;
            delay("inputKey_" + code.ToString(), frame, () => {
                inputs[(int)code] = false;
            });
        }

        public void inputHorizontal(float x, int frame = 1) {
            horizontal_input = x;
            delay("inputHorizontal", frame, () => {
                horizontal_input = 0.0f;
            });
        }

        public void inputVertical(float y, int frame = 1) {
            vertical_input = y;
            delay("inputVertical", frame, () => {
                vertical_input = 0.0f;
            });
        }

        public string getStringButtonInputs() {
            List<int> ids = new List<int>();
            for ( int i = 0; i < (int)InputCode.len; i++ ) {
                if ( inputs[i] )
                    ids.Add(i);
            }
            string s = "";
            ids.ForEach(i => {
                s += Enum.ToObject(typeof(InputCode), i);
                s += ", ";
            });
            return s;
        }

        public string getStringDirectionalInputs() {
            return "Horizontal: " + horizontal_input + "\nVertical: " + vertical_input;
        }
    }
}
