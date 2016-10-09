using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    public class MenuButton : GUIObjectBase, ISelectHandler {

        protected virtual void Awake() {
            setupSoundPlayer();
        }

        public virtual void OnSelect(BaseEventData event_data) {
            se(Const.ID.SE.BUTTON_SELECT);
        }
    }
}
