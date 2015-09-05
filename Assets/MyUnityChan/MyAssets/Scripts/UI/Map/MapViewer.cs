using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {
        private Player player;
        private bool on;
        private GameObject canvas;
        private GameObject map;

        void Awake() {
            player = null;
            canvas = GUIObjectBase.getCanvas(Const.Canvas.MAP_VIEWER_CANVAS);
            map = GameObject.Find("MapView");
        }

        void Start() {
            canvas.SetActive(false);
            Area[] areas = GameObject.Find("Area").GetComponentsInChildren<Area>();
            foreach ( Area area in areas ) {
                GameObject obj = Instantiate(area.gameObject).setParent(map).removeComponent<Area>();
                obj.AddComponent<MapAreaElement>();
                obj.GetComponent<MapAreaElement>().setAreaRef(area);
                obj.GetComponent<MeshRenderer>().enabled = true;
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

            if ( player.getController().keyPause() ) {
                if ( on ) {
                    PauseManager.Instance.pause(false);
                    on = false;
                    hide();
                }
                else {
                    PauseManager.Instance.pause(true);
                    on = true;
                    show();
                }
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
