using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {
        public string map_pagetab_name;

        public GameObject map { get; private set; }
        public GameObject mapview_camera { get; private set; }
        public MenuTabPage map_pagetab { get; protected set; }
        public Controller controller { get; private set; }
        public Canvas canvas { get; private set; }

        void Awake() {
            map = GetComponentInChildren<MapBuilder>().gameObject;
            mapview_camera = GetComponentInChildren<ModelViewCamera>().gameObject;
            controller = null;
        }

        void Start() {
            map_pagetab = MenuManager.getTabPage(map_pagetab_name);
            if ( map_pagetab )
                canvas = map_pagetab.GetComponent<Canvas>();
            else
                DebugManager.warn("the MapViewer Could not find MapPageTab : " + map_pagetab_name);
        }

        void Update() {
            if ( !canvas.enabled ) return;
            if ( !PauseManager.isPausing() ) return;

            control();
        }

        public void control() {
            if ( !controller ) {
                controller = GameStateManager.Instance.player_manager.controller;
            }

            int h = (int)controller.keyHorizontal();
            int v = (int)controller.keyVertical();

            if ( Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0 ) {
                // Slide map camera
                mapview_camera.transform.position -= (h * Vector3.right + v * Vector3.up) * 0.01f;
            }
        }
    }
}
