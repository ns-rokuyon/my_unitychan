using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class MenuPage : GUIObjectBase, IGUIOpenableGroup {

        [SerializeField]
        private List<GameObject> ex_openable_objects;

        public List<IGUIOpenable> openables { get; private set; }
        public List<IOpenable> ex_openables { get; private set; }
        public bool focused { get; private set; }
        public RectTransform rt { get; private set; }
        public Vector3 inited_scale { get; private set; }
        public Vector2 inited_size { get; private set; }
        public CanvasGroup canvas_group { get; private set; }

        void Awake() {
            rt = GetComponent<RectTransform>();
            canvas_group = GetComponent<CanvasGroup>();
            inited_scale = rt.localScale;
            inited_size = rt.sizeDelta;
            openables = new List<IGUIOpenable>();
            foreach ( IGUIOpenable openable in gameObject.GetComponentsInShallowChildren<IGUIOpenable>() ) {
                if ( openable.authorized(this) )
                    openables.Add(openable);
            }

            ex_openables = new List<IOpenable>();
            foreach ( GameObject obj in ex_openable_objects ) {
                IOpenable[] _os = obj.GetComponents<IOpenable>();
                foreach ( IOpenable openable in _os ) {
                    if ( openable != null && openable.authorized(this) ) {
                        ex_openables.Add(openable);
                    }
                }
            }
        }

        public void open() {
            openables.ForEach(openable => openable.open());
            ex_openables.ForEach(openable => openable.open());
            focused = true;
        }

        public void close() {
            openables.ForEach(openable => openable.close());
            ex_openables.ForEach(openable => openable.close());
            focused = false;
        }

        public void terminate() {
            close();
        }

        public bool authorized(object obj) {
            return true;
        }

        public GameObject getGameObject() {
            return gameObject;
        }

        public void onPageIn() {
        }

        public void onPageOut() {
        }

    }
}