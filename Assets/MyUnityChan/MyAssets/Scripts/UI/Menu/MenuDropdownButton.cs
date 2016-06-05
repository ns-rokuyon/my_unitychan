using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UniRx;

namespace MyUnityChan {
    public class MenuDropdownButton : MenuButton, 
        IPointerClickHandler, IPointerEnterHandler, ISubmitHandler {

        public GameObject dropdown_object;
        public Dropdown dropdown { get; private set; }
        public Text selected_label { get; private set; }

        private readonly string SELECTED_LABEL_GAMEOBJECT_NAME = "SelectedItemLabel";

        protected override void Awake() {
            base.Awake();
            dropdown = dropdown_object.GetComponent<Dropdown>();
            GameObject selected_label_object = 
                dropdown_object.transform.Find(SELECTED_LABEL_GAMEOBJECT_NAME).gameObject;
            if ( selected_label_object ) {
                selected_label = selected_label_object.GetComponent<Text>();
            }
            else {
                Debug.LogWarning("Cannot find gameobject by name(" + SELECTED_LABEL_GAMEOBJECT_NAME + ")");
            }
        }

        void Start() {
            this.ObserveEveryValueChanged(_ => dropdown.value)
                .Subscribe(value => unfocusDropdown(value));
        }

        public void unfocusDropdown(int value) {
            // Refocus parent button
            gameObject.GetComponent<Button>().Select();

            // Update SelectedItemLabel
            var op = dropdown.options[value];
            selected_label.text = op.text;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if ( dropdown ) dropdown.OnPointerClick(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if ( dropdown ) dropdown.OnPointerEnter(eventData);
        }

        public void OnSubmit(BaseEventData eventData) {
            if ( dropdown ) dropdown.OnSubmit(eventData);
        }
    }
}