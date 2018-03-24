using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace MyUnityChan {
    public class MenuManager : SingletonObjectBase<MenuManager> {
        public GameObject canvas_object;
        public GameObject pause_menu_body_object;
        public GameObject tab_title_object;
        public GameObject side_cover_object;
        public GameObject player_demo_object;
        public List<GameObject> tab_page_objects;

        public Canvas canvas { get; set; }
        public TextMeshProUGUI tab_title { get; private set; }
        public List<MenuTabPage> tab_pages { get; set; }
        public List<MenuNavbarButtonTabPage> navs { get; set; }
        public MenuTabSideCover side_cover { get; set; }
        public PlayerDemo player_demo { get; protected set; }

        public int focused_tab_index {
            get {
                return tab_pages.FindIndex(t => t.isFocused());
            }
        }

        public bool isOpenedMenu {
            get {
                return canvas.enabled;
            }
        }

        private EventSystem es;
        private readonly int initial_position_x = 160;

        void Awake() {
            es = EventSystem.current;
            tab_pages = new List<MenuTabPage>();

            canvas = canvas_object.GetComponent<Canvas>();
            side_cover = side_cover_object.GetComponent<MenuTabSideCover>();
            player_demo = player_demo_object.GetComponent<PlayerDemo>();

            if ( tab_title_object )
                tab_title = tab_title_object.GetComponent<TextMeshProUGUI>();

            navs = new List<MenuNavbarButtonTabPage>(FindObjectsOfType<MenuNavbarButtonTabPage>());

            foreach ( var t in tab_page_objects ) {
                t.SetActive(true);
                var tabpage = t.GetComponent<MenuTabPage>();
                var nav = navs.Find(n => n.tab_id == tabpage.tab_id);
                if ( nav ) {
                    tabpage.nav = nav;
                    nav.tabpage = tabpage;
                }
                tab_pages.Add(tabpage);
                tab_pages.Last().id = tab_pages.Count - 1;
            }
        }

        void Start() {
            canvas.enabled = false;

            // TODO
            delay(2, () => pause_menu_body_object.SetActive(false));
        }

        void Update() {
            if ( !canvas.enabled ) return;

            if ( GameStateManager.Instance.player_manager.controller.keyNextTab() ) {
                int next_index = focused_tab_index + 1;
                if ( next_index == tab_pages.Count )
                    next_index = 0;
                focus(next_index);
            }
            else if ( GameStateManager.Instance.player_manager.controller.keyPrevTab() ) {
                int next_index = focused_tab_index - 1;
                if ( next_index < 0 )
                    next_index = tab_pages.Count - 1;
                focus(next_index);
            }
        }

        public void focus(int next_index) {
            MenuTabPage focused_tab = tab_pages[focused_tab_index];
            MenuTabPage next_tab = tab_pages[next_index];

            focused_tab.deactivate();
            next_tab.activate();

            side_cover.reset(next_tab.side_cover_offset, next_tab.side_cover_deg);
        }

        public void focus(Const.ID.MenuTabPage tab_id) {
            int next_index = tabid2Index(tab_id);
            focus(next_index);
        }

        public int tabid2Index(Const.ID.MenuTabPage tab_id) {
            var tab = tab_pages.Find(t => t.tab_id == tab_id);
            return tab.id;
        }

        public void enter() {
            canvas.enabled = true;
            pause_menu_body_object.SetActive(true);
            focus(focused_tab_index);
        }

        public void quit() {
            es.SetSelectedGameObject(null); // unfocus current selected button
            canvas.enabled = false;
            pause_menu_body_object.SetActive(false);
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
