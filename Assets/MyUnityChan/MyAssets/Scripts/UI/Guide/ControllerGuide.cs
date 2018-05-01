using UnityEngine;
using UnityEngine.UI;
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

        private CanvasGroup _canvas_group;
        public CanvasGroup canvas_group {
            get { return _canvas_group ?? (_canvas_group = GetComponent<CanvasGroup>()); }
        }

        public void set(Controller.InputCode _code, string message, KeyConfig keyconfig = null) {
            code = _code;
            message_text.text = message;
            if ( no_symbol_text )
                return;
            if ( keyconfig == null )
                keyconfig = GameStateManager.controller.keyconfig;
            symbol_text.text = keyconfig.symbol(code);
        }

        public void close() {
            message_text.enabled = false;
            symbol_text.enabled = false;
            canvas_group.alpha = 0;
        }

        public void open() {
            message_text.enabled = true;
            symbol_text.enabled = true;
            canvas_group.alpha = 1;
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
