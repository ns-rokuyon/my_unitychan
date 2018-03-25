using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyUnityChan {
    [Serializable]
    public class DefaultControllerKeyConfig : KeyConfig {
        [SerializeField]
        private List<KeySlotItem> keys = new List<KeySlotItem>();

        private Dictionary<Controller.InputCode, KeySlot> _mapping = new Dictionary<Controller.InputCode, KeySlot>();
        public Dictionary<Controller.InputCode, KeySlot> mapping {
            get {
                doPrevInterval("Sync Mapping Cache", 10, () => {
                    _mapping = keys.ToDictionary(item => item.code, item => item.slot);
                });
                return _mapping;
            }
        }

        public override IEnumerable<Controller.InputCode> codes {
            get { return mapping.Keys; }
        }

        [Serializable]
        public class KeySlot : ButtonSlot {
            [SerializeField]
            public Keyboard.Key keymap;

            [SerializeField]
            public Func<string, bool> reader;

            public KeySlot(Controller.InputCode code, Keyboard.Key _keymap, Func<string, bool> _reader) : base(code) {
                keymap = _keymap;
                reader = _reader;
            }

            public override bool read() {
                return reader(keymap.sign);
            }
        }

        [Serializable]
        public class KeySlotItem : KV<Controller.InputCode, KeySlot> {
            public KeySlotItem(Controller.InputCode code, KeySlot slot) : base(code, slot) {
            }

            public Controller.InputCode code {
                get { return _1; }
            }

            public KeySlot slot {
                get { return _2; }
            }
        }

        public override void setDefault() {
            setDefaultKeyMap(Controller.InputCode.ATTACK, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.JUMP, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.PROJECTILE, Input.GetKey);
            setDefaultKeyMap(Controller.InputCode.WEAPON, Input.GetKey);
            setDefaultKeyMap(Controller.InputCode.DASH, Input.GetKey);
            setDefaultKeyMap(Controller.InputCode.GUARD, Input.GetKey);
            setDefaultKeyMap(Controller.InputCode.SWITCH_BEAM, Input.GetKey);
            setDefaultKeyMap(Controller.InputCode.GRAPPLE, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.PAUSE, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.TRANSFORM, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.TEST, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.CANCEL, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.PREV_TAB, Input.GetKeyDown);
            setDefaultKeyMap(Controller.InputCode.NEXT_TAB, Input.GetKeyDown);
        }

        public void setKeyMap(Controller.InputCode code, Keyboard.Key key, Func<string, bool> read_func) {
            KeySlot slot;
            if ( mapping.ContainsKey(code) ) {
                slot = mapping[code];
                slot.keymap = key;
                slot.reader = read_func;
                return;
            }

            slot = new KeySlot(code, key, read_func);
            keys.Add(new KeySlotItem(code, slot));
        }

        public void setDefaultKeyMap(Controller.InputCode code, Func<string, bool> read_func) {
            Keyboard.Key default_key = getDefaultKey(code);
            setKeyMap(code, default_key, read_func);
        }

        public static Keyboard.Key getDefaultKey(Controller.InputCode code) {
            return Keyboard.KeyGroups[code][0];
        }

        public override bool read(Controller.InputCode code) {
            return mapping[code].read();
        }

        public override string symbol(Controller.InputCode code) {
            return mapping[code].keymap.symbol;
        }
    }
}