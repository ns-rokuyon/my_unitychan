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
            sound.play(Const.Sound.SE.UI["BUTTON_SELECT"], true);
        }
    }
}
