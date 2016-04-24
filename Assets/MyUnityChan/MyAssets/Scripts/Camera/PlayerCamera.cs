using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        public PlayerManager player_manager { get; set; }

        private GameObject player = null;
        private Player player_component = null;

        private float area_limit_offset = 2.0f;

        private PlayerCameraPosition default_camera_position;
        private PlayerCameraPosition now_camera_position;
        private Dictionary<ViewportCorner, Vector3> viewport_corners_in_world;
        private Camera camera_component;

        // Temp flags
        private bool keep_zooming;

        enum ViewportCorner {
            LEFT_TOP,
            LEFT_BOTTOM,
            RIGHT_TOP,
            RIGHT_BOTTOM
        };

        // Use this for initialization
        void Awake() {
            GameObject.Find(Const.Canvas.GAME_CAMERA_CANVAS).GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            GameObject.Find(Const.Canvas.MAP_VIEWER_CANVAS).GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();

            camera_component = GetComponent<Camera>();

            default_camera_position = PlayerCameraPosition.getDefault();
            now_camera_position = default_camera_position;

            viewport_corners_in_world = new Dictionary<ViewportCorner, Vector3>();

            keep_zooming = false;
        }

        // Update is called once per frame
        void Update() {
            player = player_manager.getNowPlayer();
            player_component = player.GetComponent<Player>();

            updateViewpointCornersInWorld();

            if ( player ) {
                Vector3 newpos = player.transform.position + 
                    now_camera_position.position_diff + player_component.player_camera_position.position_diff;
                newpos = adjustNewPosition(newpos);

                transform.position = newpos;
            }
        }

        void OnDrawGizmos() {
            if ( viewport_corners_in_world != null ) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(viewport_corners_in_world[ViewportCorner.RIGHT_TOP], 0.1F);
                Gizmos.DrawSphere(viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM], 0.1F);
                Gizmos.DrawSphere(viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM], 0.1F);
                Gizmos.DrawSphere(viewport_corners_in_world[ViewportCorner.LEFT_TOP], 0.1F);
            }
        }

        private Vector3 adjustNewPosition(Vector3 newpos) {
            Area area = AreaManager.Instance.getArea(player_component.getAreaName());
            if ( area == null ) {
                return newpos;
            }

            newpos = limitAreaBound(newpos, area);

            if ( area.isAutoZoom() ) {
                newpos = autoZoom(newpos);
            }

            if ( area.limit_by_viewport_corners ) {
                newpos = limitByViewportCorners(newpos);
            }

            return newpos;
        }

        public void setPlayer(GameObject target) {
            // set target gameobject
            /*
            player = target;
            player_component = player.GetComponent<Player>();
            transform.position = player.transform.position + now_camera_position.position_diff;
            */
        }

        public void warpByPlayer(Player player) {
            transform.position = player.gameObject.transform.position;
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

        private void updateViewpointCornersInWorld() {
            float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

            viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM] =
                camera_component.ViewportToWorldPoint(new Vector3(0, 0, dist));
            viewport_corners_in_world[ViewportCorner.LEFT_TOP] = 
                camera_component.ViewportToWorldPoint(new Vector3(0, 1, dist));
            viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM] =
                camera_component.ViewportToWorldPoint(new Vector3(1, 0, dist));
            viewport_corners_in_world[ViewportCorner.RIGHT_TOP] =
                camera_component.ViewportToWorldPoint(new Vector3(1, 1, dist));

            shiftViewpointCornersInWorld();
        }

        private void shiftViewpointCornersInWorld() {
            float diff = 0.2f;

            viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM] =
                viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM].changeY(player.transform.position.y);
            viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM] =
                viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM].changeY(player.transform.position.y);

            viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM] =
                viewport_corners_in_world[ViewportCorner.LEFT_BOTTOM].add(diff, diff, 0.0f);
            viewport_corners_in_world[ViewportCorner.LEFT_TOP] =
                viewport_corners_in_world[ViewportCorner.LEFT_TOP].add(diff * 2.0f, -diff, 0.0f);
            viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM] =
                viewport_corners_in_world[ViewportCorner.RIGHT_BOTTOM].add(-diff, diff, 0.0f);
            viewport_corners_in_world[ViewportCorner.RIGHT_TOP] =
                viewport_corners_in_world[ViewportCorner.RIGHT_TOP].add(-diff * 2.0f, -diff, 0.0f);
        }

        private Vector3 limitAreaBound(Vector3 newpos, Area area) {
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
            return newpos;
        }

        private Vector3 limitByViewportCorners(Vector3 newpos) {
            var outOfAreas = viewport_corners_in_world.Where(pair => !Physics.Raycast(pair.Value, Vector3.forward, 20.0f)).ToList();

            outOfAreas.ForEach(pair => {
                if ( pair.Key == ViewportCorner.RIGHT_TOP ) {
                    newpos = newpos.changeY(transform.position.y < newpos.y ? transform.position.y : newpos.y)
                                   .changeX(transform.position.x < newpos.x ? transform.position.x : newpos.x);
                }
                else if ( pair.Key == ViewportCorner.LEFT_TOP ) {
                    newpos = newpos.changeY(transform.position.y < newpos.y ? transform.position.y : newpos.y)
                                   .changeX(transform.position.x > newpos.x ? transform.position.x : newpos.x);
                }
                /*
                if ( pair.Key == ViewportCorner.RIGHT_BOTTOM ) {
                    newpos = newpos.changeY(transform.position.y > newpos.y ? transform.position.y : newpos.y)
                                   .changeX(transform.position.x < newpos.x ? transform.position.x : newpos.x);
                }
                if ( pair.Key == ViewportCorner.LEFT_BOTTOM ) {
                    newpos = newpos.changeY(transform.position.y > newpos.y ? transform.position.y : newpos.y)
                                   .changeX(transform.position.x > newpos.x ? transform.position.x : newpos.x);
                }
                */
            });

            return newpos;
        }

        private Vector3 autoZoom(Vector3 newpos) {
            var outOfAreas = viewport_corners_in_world.Where(pair => !Physics.Raycast(pair.Value, Vector3.forward, 20.0f)).ToList();
            if ( !keep_zooming && outOfAreas.Count == 0 ) {
                return newpos;
            }

            StartCoroutine(switchZoomInterval());

            // Zoom
            return newpos.add(0, -1.0f, 2.0f);
        }

        private IEnumerator switchZoomInterval() {
            if ( keep_zooming ) yield break;
            keep_zooming = true;
            yield return new WaitForSeconds(1.0f);
            keep_zooming = false;
        }

    }
}
