using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
            len
        };

        protected Character self;

        protected List<bool> inputs;
        protected List<bool> raw_inputs;
        protected float horizontal_input;
        protected float vertical_input;

        public IObservable<float> keyStreamHorizontal { get; private set; }
        public IObservable<float> keyStreamVertical { get; private set; }

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
            vertical_input = 0.0f;

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
                    buttons_debug_text.text = "";
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
                horizontal_input = 0.0f;
                vertical_input = 0.0f;
            }
        }

        public CommandRecorder getCommandRecorder() {
            return GetComponent<CommandRecorder>();
        }

        public bool getRawInput(InputCode code) {
            int c = (int)code;
            return raw_inputs[c] || inputs[c];
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
        public float keyHorizontal() { return horizontal_input; }
        public float keyVertical() { return vertical_input; }

        public IEnumerator pressAndReleaseKey(InputCode code, int frame) {
            while ( frame > 0 ) {
                inputs[(int)code] = true;
                frame--;
                yield return null;
            }
            inputs[(int)code] = false;
        }

        public IEnumerator pressAndReleaseHorizontal(float x, int frame) {
            while ( frame > 0 ) {
                horizontal_input = x;
                frame--;
                yield return null;
            }
            horizontal_input = 0.0f;
        }

        public IEnumerator pressAndReleaseVertical(float y, int frame) {
            while ( frame > 0 ) {
                vertical_input = y;
                frame--;
                yield return null;
            }
            vertical_input = 0.0f;
        }

        public void inputKey(InputCode code, int frame = 1) {
            if ( inputs[(int)code] )
                return;
            StartCoroutine(pressAndReleaseKey(code, frame));
        }

        public void inputHorizontal(float x, int frame = 1) {
            if ( horizontal_input > 0.0f || horizontal_input < 0.0f )
                return;
            StartCoroutine(pressAndReleaseHorizontal(x, frame));
        }

        public void inputVertical(float y, int frame = 1) {
            if ( vertical_input > 0.0f || vertical_input < 0.0f )
                return;
            StartCoroutine(pressAndReleaseVertical(y, frame));
        }

        public string getStringDirectionalInputs() {
            return "Horizontal: " + horizontal_input + "\nVertical: " + vertical_input;
        }
    }
}
