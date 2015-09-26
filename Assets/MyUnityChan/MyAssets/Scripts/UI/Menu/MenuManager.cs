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

            if ( canvas_object.activeSelf == false ) {
                canvas_object.SetActive(true);
            }

            selectables = new List<Selectable>();
            foreach ( Selectable selectable in canvas_object.GetComponentsInChildren<Selectable>() ) {
                selectables.Add(selectable);
            }

            canvas_object.SetActive(false);
        }

        void Update() {
        }

        public void enter() {
            canvas_object.SetActive(true);
            selectables.First<Selectable>().Select();
        }

        public void quit() {
            es.SetSelectedGameObject(null); // unfocus current selected button
            canvas_object.SetActive(false);
        }

        public void doQuit() {
            quit();
            GameStateManager.change(GameStateManager.GameState.MAIN);
            PauseManager.Instance.pause(false);
        }

        public void doMap() {
            quit();
            GameStateManager.change(GameStateManager.GameState.MAP);
            PauseManager.setPauseControlMethod(MapViewer.Instance.control);
        }
    }
}
