using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class MenuButton : GUIObjectBase, ISelectHandler {

        void Awake() {
            setupSoundPlayer();
        }

        public void OnSelect(BaseEventData event_data) {
            iTween.ScaleTo(this.gameObject, iTween.Hash("x", 1.05f, "y", 1.05f, "z", 1.05f, "time", 0.05f));
            iTween.ScaleTo(this.gameObject, iTween.Hash("x", 1.0f, "y", 1.0f, "z", 1.0f, "time", 0.05f, "delay", 0.1f));
            sound.play(Const.Sound.SE.UI["BUTTON_SELECT"], true);
        }
    }
}
