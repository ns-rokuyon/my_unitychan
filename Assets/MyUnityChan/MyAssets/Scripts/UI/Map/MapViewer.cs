using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {

        public GameObject map_pagetab;

        public GameObject mapview_camera { get; private set; }
        public GameObject map { get; private set; }
        public Controller controller { get; private set; }
        public Canvas canvas { get; private set; }

        void Awake() {
            map = GetComponentInChildren<MapBuilder>().gameObject;
            mapview_camera = GetComponentInChildren<ModelViewCamera>().gameObject;
            controller = null;
            canvas = map_pagetab.GetComponent<Canvas>();
        }

        void Update() {
            if ( !canvas.enabled ) return;

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
