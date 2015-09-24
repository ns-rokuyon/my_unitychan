using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class MenuButton : GUIObjectBase, ISelectHandler {

        public void OnSelect(BaseEventData event_data) {
            iTween.ShakeRotation(this.gameObject, iTween.Hash("z", 10, "time", 0.4f));
        }
    }
}
