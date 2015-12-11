using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        private GameObject player = null;
        private Player player_component = null;

        private float area_limit_offset = 2.0f;

        private PlayerCameraPosition default_camera_position;
        private PlayerCameraPosition now_camera_position;

        // Use this for initialization
        void Awake() {
            GameObject.Find(Const.Canvas.GAME_CAMERA_CANVAS).GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            GameObject.Find(Const.Canvas.MAP_VIEWER_CANVAS).GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();

            default_camera_position = PlayerCameraPosition.getDefault();
            now_camera_position = default_camera_position;
            Debug.Log(now_camera_position.position_diff);
        }

        // Update is called once per frame
        void Update() {
            if ( player ) {
                Area area = AreaManager.Instance.getArea(player_component.getAreaName());
                Vector3 newpos = player.transform.position + now_camera_position.position_diff;
                if ( area ) {
                    if ( area.right_wall && newpos.x + area_limit_offset >= area.limitRight() ) {
                        newpos.x = transform.position.x;    // no update
                    }
                    if ( area.left_wall && newpos.x - area_limit_offset <= area.limitLeft() ) {
                        newpos.x = transform.position.x;    // no update
                    }
                    if ( area.roof && newpos.y + area_limit_offset >= area.limitRoof() ) {
                        newpos.y = transform.position.y;    // no update
                    }
                    if ( area.floor && newpos.y - area_limit_offset <= area.limitFloor() ) {
                        newpos.y = transform.position.y;    // no update
                    }
                }
                transform.position = newpos;
            }
        }

        public void setPlayer(GameObject target) {
            // set target gameobject
            player = target;
            player_component = player.GetComponent<Player>();
            transform.position = player.transform.position + now_camera_position.position_diff;
        }

        public void setPositionInArea(Area area) {
            PlayerCameraPosition pos = area.camera_position;
            if ( pos.on ) {
                now_camera_position = pos;
            }
            else {
                now_camera_position = default_camera_position;
            }

            gameObject.transform.rotation = Quaternion.Euler(now_camera_position.rotation);
        }
    }
}
