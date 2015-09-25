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
            canvas.transform.FindChild("BackGround").gameObject.SetActive(true);
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
                    // Create area joint point into mapviewer
                    GameObject pointA = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], map);
                    GameObject pointB = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], map);
                    pointA.transform.position = conn.pointA;
                    pointB.transform.position = conn.pointB;

                    // Create area connection edge into mapviewer
                    GameObject connection_edge = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_LINE"], map);
                    Mesh mesh = new Mesh();
                    Vector3[] vertices = new Vector3[] {
                        conn.pointA + Vector3.down, conn.pointB + Vector3.down, conn.pointA + Vector3.up, conn.pointB + Vector3.up
                    };      // 4 vertices as plane
                    Vector2[] uv = new Vector2[] {
                        Vector2.zero, Vector2.right, Vector2.up, Vector2.one
                    };
                    int[] triangles = new int[] {
                        0, 1, 2,
                        3, 2, 1
                    };
                    mesh.vertices = vertices;
                    mesh.uv = uv;
                    mesh.triangles = triangles;
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();
                    connection_edge.GetComponent<MeshFilter>().sharedMesh = mesh;
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

            if ( downtime_count == 0 && GameStateManager.now() == GameStateManager.GameState.MAP ) {
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
