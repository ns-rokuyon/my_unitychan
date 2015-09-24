using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MenuManager : SingletonObjectBase<MenuManager> {
        public GameObject canvas_object;

        private EventSystem es;
        private List<Selectable> selectables;

        void Awake() {
            es = EventSystem.current;

            selectables = new List<Selectable>();
            foreach ( Selectable selectable in canvas_object.GetComponentsInChildren<Selectable>() ) {
                selectables.Add(selectable);
            }
        }

        void Update() {
        }
    }
}
