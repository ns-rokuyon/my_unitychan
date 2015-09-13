﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MapViewer : SingletonObjectBase<MapViewer> {
        private Player player;
        public bool on;
        private GameObject canvas;
        private GameObject map;

        private int downtime_count = 2;

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
                obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/MapAreaElementMat");
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
            if ( Input.GetKeyDown("p") ) {
                PauseManager.Instance.pause(false);
                on = false;
                downtime_count = 2;
                hide();
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
