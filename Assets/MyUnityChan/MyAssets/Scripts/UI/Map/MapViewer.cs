using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {
        private Player player;
        public bool on;
        private GameObject canvas;
        private GameObject map;

        private int downtime_count = 10;

        void Awake() {
            player = null;
            canvas = GUIObjectBase.getCanvas(Const.Canvas.MAP_VIEWER_CANVAS);
            map = GameObject.Find("MapView");
        }

        void Start() {
            canvas.SetActive(false);

            Area[] areas = GameObject.Find("Area").GetComponentsInChildren<Area>();
            List<Tuple<GameObject,GameObject>> map_areaconnections = new List<Tuple<GameObject,GameObject>>();
            foreach ( Area area in areas ) {
                GameObject obj = Instantiate(area.gameObject).setParent(map).removeComponent<Area>();
                obj.AddComponent<MapAreaElement>();
                obj.GetComponent<MapAreaElement>().setAreaRef(area);
                obj.GetComponent<MeshRenderer>().enabled = true;
                obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/MapAreaElementMat");

                List<AreaConnection> connections = area.getAreaConnections();
                foreach ( AreaConnection conn in connections ) {
                    GameObject pointA = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], map);
                    GameObject pointB = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], map);
                    pointA.transform.position = conn.pointA;
                    pointB.transform.position = conn.pointB;
                    map_areaconnections.Add(new Tuple<GameObject, GameObject>(pointA, pointB));
                }
            }
            map.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

            map.SetActive(false);
        }

        void Update() {
            if ( player == null ) {
                GameObject obj = Player.getPlayer();
                if ( obj != null ) {
                    player = obj.GetComponent<Player>();
                }
                return;
            }

            if ( downtime_count == 0 && player.getController().keyPause() ) {
                if ( !on ) {
                    PauseManager.Instance.pause(true, control);
                    on = true;
                    show();
                }
            }

            downtime_count--;
            if ( downtime_count < 0 ) {
                downtime_count = 0;
            }
        }

        public void control() {
            if ( player.getController().keyPause() ) {
                // If key 'p' is pressed, close mapviewer
                PauseManager.Instance.pause(false);
                on = false;
                downtime_count = 10;
                hide();
                return;
            }

            int h = (int)player.getController().keyHorizontal();
            int v = (int)player.getController().keyVertical();

            if ( Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0 ) {
                map.transform.position += (h * Vector3.right + v * Vector3.up) * 0.01f;
            }
        }

        private void hide() {
            canvas.SetActive(false);
            map.SetActive(false);
        }

        private void show() {
            canvas.SetActive(true);
            map.SetActive(true);

            map.transform.position = player.getPlayerCamera().gameObject.transform.position + Vector3.forward * 0.7f;
        }

        public bool isOn() {
            return on;
        }

    }
}
