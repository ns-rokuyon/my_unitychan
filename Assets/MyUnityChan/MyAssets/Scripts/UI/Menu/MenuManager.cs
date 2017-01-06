using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MenuManager : SingletonObjectBase<MenuManager> {
        public GameObject canvas_object;
        public GameObject tab_title_object;
        public List<GameObject> tab_page_objects;

        public Canvas canvas { get; set; }
        public Text tab_title { get; private set; }
        public List<MenuTabPage> tab_pages { get; set; }

        private EventSystem es;
        private readonly int initial_position_x = 160;

        void Awake() {
            es = EventSystem.current;
            tab_pages = new List<MenuTabPage>();

            canvas = canvas_object.GetComponent<Canvas>();

            if ( tab_title_object )
                tab_title = tab_title_object.GetComponent<Text>();

            foreach ( var t in tab_page_objects ) {
                t.SetActive(true);
                tab_pages.Add(t.GetComponent<MenuTabPage>());
                tab_pages.Last().id = tab_pages.Count - 1;
            }

            canvas.enabled = false;
        }

        void Update() {
            if ( !canvas.enabled ) return;

            if ( GameStateManager.Instance.player_manager.controller.keyNextTab() ) {
                int focused_tab_id = tab_pages.FindIndex(t => t.isFocused());
                int next_id = focused_tab_id + 1;
                if ( next_id == tab_pages.Count )
                    next_id = 0;
                tab_pages[focused_tab_id].deactivate();
                tab_pages[next_id].activate();
            }
            else if ( GameStateManager.Instance.player_manager.controller.keyPrevTab() ) {
                int focused_tab_id = tab_pages.FindIndex(t => t.isFocused());
                int next_id = focused_tab_id - 1;
                if ( next_id < 0 )
                    next_id = tab_pages.Count - 1;
                tab_pages[focused_tab_id].deactivate();
                tab_pages[next_id].activate();
            }
        }

        public void enter() {
            canvas.enabled = true;
        }

        public void quit() {
            es.SetSelectedGameObject(null); // unfocus current selected button
            canvas.enabled = false;
        }

        public void suspend() {
        }

        public void doQuit() {
            quit();
            GameStateManager.change(GameStateManager.GameState.MAIN);
            PauseManager.Instance.pause(false);
        }

        public static MenuTabPage getTabPage(string name) {
            return self().tab_pages.FirstOrDefault(p => p.tab_name.en == name);
        }

        public static string getCurrentSelectedName() {
            if ( self().canvas && self().canvas.enabled ) {
                var obj = self().es.currentSelectedGameObject;
                if ( obj )
                    return obj.name;
                return "No selected ui objects";
            }
            return "Canvas is disabled";
        }
    }
}
