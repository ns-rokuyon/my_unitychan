using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    [RequireComponent(typeof(Canvas))]
    public class MenuTabPage : GUIObjectBase {
        [SerializeField]
        public GameText tab_name;

        public int id { get; set; }
        public List<Selectable> selectables { get; private set; }
        private Canvas canvas;
        private EventSystem es;

        void Awake() {
            es = EventSystem.current;
            canvas = GetComponent<Canvas>();
            selectables = new List<Selectable>();

            foreach ( Selectable selectable in canvas.GetComponentsInChildren<Selectable>() ) {
                selectables.Add(selectable);
            }
        }

        void Start() {
            if ( id == 0 )
                activate();
            else
                deactivate();
        }

        public bool isFocused() {
            return canvas.enabled;
        }

        public void activate() {
            MenuManager.self().tab_title.text = tab_name.get();
            canvas.enabled = true;
            var first_selectable = selectables.FirstOrDefault();
            if ( first_selectable )
                es.SetSelectedGameObject(first_selectable.gameObject);
        }

        public void deactivate() {
            canvas.enabled = false;
        }
    }
}
