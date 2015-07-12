﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyUnityChan {
    public class ModalManager : SingletonObjectBase<ModalManager> {
        public GameObject canvas;
        public GameObject modal;
        public Text modal_title;
        public Text modal_desc;

        void Start() {
            canvas = FindObjectOfType<Canvas>().gameObject;
            modal = canvas.transform.FindChild("ModalPanel").gameObject;
            modal_title = modal.transform.FindChild("Title").gameObject.GetComponent<Text>();
            modal_desc = modal.transform.FindChild("Description").gameObject.GetComponent<Text>();
        }

        public void show(string title, string desc) {
            modal_title.text = title;
            modal_desc.text = desc;
            modal.SetActive(true);
        }

        public void hide() {
            modal_title.text = "";
            modal_desc.text = "";
            modal.SetActive(false);
        }

    }
}