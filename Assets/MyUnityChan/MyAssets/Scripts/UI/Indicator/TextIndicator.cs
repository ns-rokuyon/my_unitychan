using UnityEngine;
using System.Collections;
using System;
using TMPro;

namespace MyUnityChan {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextIndicator : GUIObjectBase, IGUIOpenable {
        public bool allow_tabpage_to_open_me;

        private TextMeshProUGUI text;

        void Awake() {
            text = GetComponent<TextMeshProUGUI>();
            close();
        }

        public void close() {
            if ( !text )
                return;
            text.enabled = false;
        }

        public void open() {
            if ( !text )
                return;
            text.enabled = true;
        }

        public void terminate() {
            close();
        }

        public void setMessage(string message) {
            text.text = message;
        }

        public void setMessage(GameText message) {
            setMessage(message.get());
        }

        public bool authorized(object obj) {
            if ( allow_tabpage_to_open_me )
                return true;

            Type t = obj.GetType();
            if ( t == typeof(MenuTabPage) )
                return false;
            return true;
        }

        public GameObject getGameObject() {
            return gameObject;
        }
    }
}