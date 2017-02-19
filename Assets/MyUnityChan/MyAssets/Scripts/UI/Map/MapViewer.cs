using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using UniRx;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {
        public string map_pagetab_name;

        public ModelViewCamera mapview_camera { get; private set; }
        public MenuTabPage map_pagetab { get; protected set; }
        public Controller controller { get; private set; }
        public Canvas canvas { get; private set; }
        public MapBuilder map { get; private set; }

        public RectTransform render_image_position { get; private set; }
        public RectTransform player_position { get; private set; }

        public PlayerManager pm {
            get {
                return GameStateManager.Instance.player_manager;
            }
        }

        public MapAreaElement now_area_element {
            get {
                if ( pm.area_name == null )
                    return null;

                MapAreaElement e = map.findAreaElementByAreaName(pm.area_name);
                if ( !e ) {
                    DebugManager.warn("Could not find a MapAreaElement name=" + pm.area_name);
                    return null;
                }
                return e;
            }
        }

        void Awake() {
            map = GetComponentInChildren<MapBuilder>();
            mapview_camera = GetComponentInChildren<ModelViewCamera>();
            controller = null;
        }

        void Start() {
            map_pagetab = MenuManager.getTabPage(map_pagetab_name);
            if ( map_pagetab )
                canvas = map_pagetab.GetComponent<Canvas>();
            else
                DebugManager.warn("the MapViewer Could not find MapPageTab : " + map_pagetab_name);

            if ( canvas )
                this.ObserveEveryValueChanged(mv => PauseManager.isPausing() || mv.canvas.enabled)
                    .Where(b => b)
                    .Subscribe(_ => focusNowArea());

            if ( map_pagetab ) {
                render_image_position = map_pagetab.findUIObjectInChildren("MapImage").GetComponent<RectTransform>();
                player_position = map_pagetab.findUIObjectInChildren("PlayerPositionIcon").GetComponent<RectTransform>();
            }
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

            var now_area = now_area_element;
            if ( now_area ) {
                player_position.anchoredPosition = 
                    convertWorldPositionToScreenPositionOverRenderImage(now_area.transform.position);
            }

            int h = (int)controller.keyHorizontal();
            int v = (int)controller.keyVertical();

            if ( Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0 ) {
                // Slide map camera
                //mapview_camera.transform.position -= (h * Vector3.right + v * Vector3.up) * 0.01f;

                // Slide map parent object
                map.transform.position -= (h * Vector3.right + v * Vector3.up) * 0.01f;
            }
        }

        public void focusNowArea() {
            var e = now_area_element;
            if ( !e )
                return;
        }

        public Vector2 convertWorldPositionToScreenPositionOverRenderImage(Vector3 world) {
            Vector2 vp = mapview_camera.getCamera().WorldToViewportPoint(world);
            float baseWidth = render_image_position.rect.width;
            float baseHeight = render_image_position.rect.height;
            return new Vector2(vp.x * baseWidth - baseWidth / 2, vp.y * baseHeight - baseHeight / 2);
        }
    }
}
