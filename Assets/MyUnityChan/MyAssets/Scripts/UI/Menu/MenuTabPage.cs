using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace MyUnityChan {
    [RequireComponent(typeof(Canvas))]
    public class MenuTabPage : GUIObjectBase {
        [SerializeField]
        public GameText tab_name;
        public GameObject prior_selectable;
        public Const.ID.MenuTabPage tab_id;

        public float side_cover_offset;
        public float side_cover_deg;

        public List<Vector2> slide_offsets = new List<Vector2>();

        public int id { get; set; }
        public List<Selectable> selectables { get; private set; }
        public List<IGUIOpenable> openables { get; private set; }
        public List<MenuPager> pagers { get; private set; }
        public MenuNavbarButtonTabPage nav { get; set; }
        public int slide_index { get; private set; }
        private Canvas canvas;
        private EventSystem es;

        void Awake() {
            es = EventSystem.current;
            canvas = GetComponent<Canvas>();
            selectables = new List<Selectable>();
            openables = new List<IGUIOpenable>();
            pagers = new List<MenuPager>();
            slide_index = 0;

            foreach ( Selectable selectable in canvas.GetComponentsInChildren<Selectable>() ) {
                selectables.Add(selectable);
            }

            foreach ( IGUIOpenable openable in canvas.GetComponentsInShallowChildren<IGUIOpenable>() ) {
                var cs = openable.getGameObject().GetComponentsInParent<IGUIOpenableGroup>();
                if ( cs.Length > 0 ) {
                    // This openable is under IGUIOpenableGroup
                    continue;
                }

                if ( openable.authorized(this) ) {
                    openables.Add(openable);

                    if ( openable is MenuPager ) {
                        pagers.Add(openable as MenuPager);
                    }
                }
            }

            if ( slide_offsets.Count == 0 || slide_offsets[0] != Vector2.zero ) {
                slide_offsets.Insert(0, Vector2.zero);
            }
        }

        public bool isFocused() {
            return canvas.enabled;
        }

        public void activate() {
            MenuManager.self().tab_title.text = tab_name.get();
            canvas.enabled = true;

            openables.ForEach(openable => openable.open());

            if ( prior_selectable ) {
                es.SetSelectedGameObject(prior_selectable);
                return;
            }
            var first_selectable = selectables.FirstOrDefault();
            if ( first_selectable )
                es.SetSelectedGameObject(first_selectable.gameObject);
        }

        public void deactivate() {
            canvas.enabled = false;
            es.SetSelectedGameObject(null);

            openables.ForEach(openable => openable.terminate());
        }

        public GameObject findUIObjectInChildren(string name) {
            return GameObject.Find(Hierarchy.getHierarchyPath(this.gameObject) + "/" + name);
        }

        public void goNextPage() {
            pagers.ToList().ForEach(pager => pager.goNextPage());
        }

        public void goPrevPage() {
            pagers.ToList().ForEach(pager => pager.goPrevPage());
        }
    }
}
