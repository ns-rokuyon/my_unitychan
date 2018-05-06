using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;
using UniRx.Triggers;


namespace MyUnityChan {
    public class QuickSelector : GUIObjectBase {

        // Whether this selector is selectable now or not
        public bool opened { get; protected set; }

        // The button which is corresponding to the selected value currently
        // This selected button keeps being displayed.
        public Button selected_button { get; protected set; } 
        
        public EventSystem es {
            get {
                return EventSystem.current;
            }
        }

        public virtual bool isPressedKey {
            get {
                return GameStateManager.getPlayer().manager.controller.keySwitchBeam();
            }
        }

        public virtual string button_prefab_path {
            get {
                return "";
            }
        }

        public virtual string selected_button_prefab_path {
            get {
                return button_prefab_path;
            }
        }

        public List<Button> buttons {
            get {
                return content.GetComponentsInChildren<Button>().ToList();
            }
        }

        public Const.ID.UIVerticalDirection append_direction {
            get {
                return (int)layout_group.childAlignment <= 2 ? Const.ID.UIVerticalDirection.DOWN : Const.ID.UIVerticalDirection.UP;
            }
        }


        protected ScrollRect scroll_rect;
        protected GameObject content;
        protected VerticalLayoutGroup layout_group;
        protected IDisposable opener;
        protected IDisposable closer;

        void Awake() {
            scroll_rect = GetComponent<ScrollRect>();
            if ( scroll_rect ) {
                content = scroll_rect.content.gameObject;
                layout_group = content.GetComponent<VerticalLayoutGroup>();
            }
        }

        void Start() {
            init();
            setOpener();

            this.UpdateAsObservable()
                .Where(_ => opened)
                .Subscribe(_ => onFocus())
                .AddTo(this);

            delay(5, () => {
                open();
                close();
            });
        }

        protected virtual void init() {
            // Initialize
        }

        protected virtual void setupButtons() {
            // Instantiate buttons into content
            // This method will be called once when 'isPressedKey' returns true
        }

        protected virtual void onClose() {
            // This method will be called once when 'isPressedKey' returns false
        }

        protected virtual Button getFirstSelected() {
            // Return the button which is selected first
            return buttons.First();
        }

        protected virtual void onFocus() {
            // This method is called while the selector is opened
            if ( !selected_button )
                return;
            es.SetSelectedGameObject(selected_button.gameObject);
        }

        public virtual void onSelect(BaseEventData data) {
            // Callback method which a selector button calls when it is selected 
            selected_button = data.selectedObject.GetComponent<Button>();
        }

        protected void setOpener() {
            if ( opener != null ) {
                opener.Dispose();
                opener = null;
            }
            opener = this.UpdateAsObservable()
                .Where(_ => !opened)
                .Where(_ => isPressedKey)
                .Take(5)
                .Subscribe(_ => open())
                .AddTo(this);
        }

        protected void setCloser() {
            if ( closer != null ) {
                closer.Dispose();
                closer = null;
            }
            closer = this.UpdateAsObservable()
                .Where(_ => opened)
                .Where(_ => !isPressedKey)
                .Take(5)
                .Subscribe(_ => {
                    close();
                    setOpener();
                })
                .AddTo(this);
        }

        protected void open() {
            opened = true;
            setupButtons();
            selected_button = getFirstSelected();
            if ( append_direction == Const.ID.UIVerticalDirection.UP )
                selected_button.transform.SetAsLastSibling();
            else
                selected_button.transform.SetAsFirstSibling();
            setCloser();
        }

        protected void close() {
            opened = false;
            onClose();
            buttons.ForEach(b => {
                if ( b != selected_button )
                    Destroy(b.gameObject);
            });
        }

        protected List<T> siblingOrder<T>(List<T> list) {
            if ( append_direction == Const.ID.UIVerticalDirection.UP ) {
                return list.OrderByDescending(t => t).ToList();
            }
            return list.OrderBy(t => t).ToList();
        }
    }
}