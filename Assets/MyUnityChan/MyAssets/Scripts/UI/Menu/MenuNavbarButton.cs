using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class MenuNavbarButton<T> : MenuButton {
        [SerializeField]
        public T nav;
        public bool change_by_select;

        public virtual void change() { }

        public override void OnSelect(BaseEventData event_data) {
            base.OnSelect(event_data);
            if ( change_by_select ) change();
        }
    }

}
