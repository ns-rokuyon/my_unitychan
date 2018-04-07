using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

namespace MyUnityChan {
    public class ControllerGuide : GUIObjectBase, IGUIOpenable {
        [SerializeField]
        public TextMeshProUGUI message_text;

        [SerializeField]
        public TextMeshProUGUI symbol_text;

        [SerializeField]
        public Controller.InputCode code;

        [SerializeField]
        private bool no_symbol_text;

        public void set(Controller.InputCode _code, string message, KeyConfig keyconfig) {
            code = _code;
            message_text.text = message;
            if ( no_symbol_text )
                return;
            symbol_text.text = keyconfig.symbol(code);
        }

        public void close() {
            message_text.enabled = false;
            symbol_text.enabled = false;
        }

        public void open() {
            message_text.enabled = true;
            symbol_text.enabled = true;
        }

        public void terminate() {
            close();
        }

        public bool authorized(object obj) {
            return true;
        }

        public GameObject getGameObject() {
            return gameObject;
        }
    }
}
