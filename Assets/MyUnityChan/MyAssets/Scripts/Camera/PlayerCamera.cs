using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        public bool tracking;
        public float tracking_margin_width;     // 0.0 - 1.0
        public float tracking_margin_height;

        public PlayerManager player_manager { get; set; }

        private GameObject player = null;
        private Player player_component = null;

        private float area_limit_offset = 2.0f;

        private PlayerCameraPosition default_camera_position;
        public PlayerCameraPosition now_camera_position { get; set; }
        private Dictionary<ViewportCorner, Vector3> viewport_corners_in_world;
        private UnityStandardAssets.ImageEffects.MotionBlur blur;
        private Camera camera_component;
        private Camera worldspace_ui_camera_component;

        public CameraEffect effect { get; protected set; }

        public float tracking_margin_width_half {
            get { return tracking_margin_width / 2.0f; }
        }

        public float tracking_margin_height_half {
            get { return tracking_margin_height / 2.0f; }
        }

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
            camera_component = GetComponent<Camera>();
            worldspace_ui_camera_component = transform.FindChild("WorldUICamera").GetComponent<Camera>();
            effect = GetComponent<CameraEffect>();

            blur = GetComponent<UnityStandardAssets.ImageEffects.MotionBlur>();

            default_camera_position = PlayerCameraPosition.getDefault();
            now_camera_position = default_camera_position;

            viewport_corners_in_world = new Dictionary<ViewportCorner, Vector3>();

            keep_zooming = false;

            GUIObjectBase.getCanvas(Const.Canvas.WORLD_SPACE_CANVAS).GetComponent<Canvas>().worldCamera = worldspace_ui_camera_component;
        }

        void Start() {
            this.ObserveEveryValueChanged(_ => AreaManager.self().now_area_name)
                .DelayFrame(20)
                .Subscribe(name => {
                    DebugManager.log(name);
                    centralize();
                });
        }

        // Update is called once per frame
        void LateUpdate() {
            player = player_manager.getNowPlayer();
            player_component = player.GetComponent<Player>();

            updateViewpointCornersInWorld();

            if ( player ) {
                if ( tracking ) {
                    Vector3 newpos;
                    if ( tracking_margin_height <= 0.0f && tracking_margin_width <= 0.0f ) {
                        centralize();
                    }
                    else {
                        Vector2 point = getPlayerViewportPoint();
                        if ( !isInTrackingMargin(point) ) {
                            var diff = getTrackingDirectionDiff(point);
                            newpos = transform.position.add(diff.x, diff.y, 0.0f);
                            transform.position = adjustNewPosition(newpos);
                        }
                    }
                }

                if ( blur ) {
                    if ( player_component.isHitstopping() ) {
                        blur.blurAmount = 0.6f;
                    }
                    else {
                        blur.blurAmount = 0.1f;
                    }
                }
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

        public Vector2 getPlayerScreenPoint() {
            if ( !player )
                return Vector2.zero;
            return camera_component.WorldToScreenPoint(player.transform.position);
        }

        public Vector2 getPlayerViewportPoint() {
            if ( !player )
                return Vector2.zero;
            return camera_component.WorldToViewportPoint(player.transform.position);
        }

        public bool isInTrackingMargin(Vector2 point) {
            float left_border = 0.5f - tracking_margin_width_half;
            float right_border = 0.5f + tracking_margin_width_half;
            float top_border = 0.5f - tracking_margin_height_half;
            float bottom_border = 0.5f + tracking_margin_height_half;

            if ( left_border < point.x && point.x < right_border && top_border < point.y && point.y < bottom_border ) {
                return true;
            }
            return false;
        }

        public Vector3 getTrackingDirectionDiff(Vector2 point) {
            float left_border = 0.5f - tracking_margin_width_half;
            float right_border = 0.5f + tracking_margin_width_half;
            float top_border = 0.5f + tracking_margin_height_half;
            float bottom_border = 0.5f - tracking_margin_height_half;

            float dx = 0.0f, dy = 0.0f;
            Vector3 diff = player_component.getLatestPositionDiff(in_lateupdate:true);

            if ( point.x < left_border )
                dx = -diff.x;
            else if ( right_border < point.x )
                dx = diff.x;

            if ( point.y < bottom_border )
                dy = -diff.y;
            else if ( top_border < point.y )
                dy = diff.y;

            return new Vector3(dx, dy, 0.0f);
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
            centralize();
            tracking = true;
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

        public void shake() {
            transform.DOShakePosition(0.1f, strength: 0.6f);
            tracking = false;
            StartCoroutine(restartTracking(0.1f));
        }

        public void zoom(Vector3 to, float duration) {
            transform.DOMove(to, duration);
            tracking = false;
        }

        public IEnumerator restartTracking(float sec) {
            yield return new WaitForSeconds(sec);
            tracking = true;
        }

        public void centralize() {
            if ( !player )
                return;
            var newpos = player.transform.position + 
                now_camera_position.position_diff + player_component.player_camera_position.position_diff;
            newpos = adjustNewPosition(newpos);
            transform.position = newpos;    // Update position
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
