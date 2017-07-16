using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyUnityChan {
    public class ModalManager : SingletonObjectBase<ModalManager> {
        public GameObject canvas { get; protected set; }
        public GameObject modal { get; protected set; }
        public Text modal_title { get; protected set; }
        public Text modal_desc { get; protected set; }

        void Start() {
            canvas = GUIObjectBase.getCanvas(Const.Canvas.GAME_CAMERA_CANVAS);
            modal = canvas.transform.Find("ModalPanel").gameObject;
            modal_title = modal.transform.Find("Title").gameObject.GetComponent<Text>();
            modal_desc = modal.transform.Find("Description").gameObject.GetComponent<Text>();
        }

        public void show(GameText title, GameText desc) {
            modal_title.text = title.get();
            modal_desc.text = desc.get();
            modal.SetActive(true);
        }

        public void hide() {
            modal_title.text = "";
            modal_desc.text = "";
            modal.SetActive(false);
        }

        public void control() {
            if ( Input.GetKeyDown("space") ) {
                PauseManager.Instance.pause(false);
            }
        }

    }
}
