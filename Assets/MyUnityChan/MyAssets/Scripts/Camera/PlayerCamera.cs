using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        public PlayerManager player_manager { get; set; }
        public CameraEffect effect { get; protected set; }

        public Camera worldspace_ui_camera_component { get; protected set; }
        public Camera camera_component { get; protected set; }

        public GameObject player {
            get {
                if ( !player_manager )
                    return null;
                return player_manager.getNowPlayer();
            }
        }
        public Player player_component {
            get {
                if ( !player )
                    return null;
                return player.GetComponent<Player>();
            }
        }

        public virtual void Awake() {
            camera_component = GetComponent<Camera>();
            worldspace_ui_camera_component = transform.FindChild("WorldUICamera").GetComponent<Camera>();
            effect = GetComponent<CameraEffect>();
            GUIObjectBase.getCanvas(Const.Canvas.WORLD_SPACE_CANVAS).GetComponent<Canvas>().worldCamera = worldspace_ui_camera_component;
        }

        public virtual void Start() {
            this.ObserveEveryValueChanged(_ => AreaManager.self().now_area_name)
                .DelayFrame(20)
                .Subscribe(name => {
                    DebugManager.log(name);
                    onChangeArea();
                });
        }

        public virtual void LateUpdate() {
        }

        public virtual void onChangeArea() {
        }

        public virtual void centralize() {
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

        public virtual Vector3 getZoomPointOnChangeArea() {
            return player.transform.position;
        }

        public virtual void setPositionInArea(Area area) {
        }

        public virtual void shake() {
        }

        public virtual void zoom(Vector3 to, float duration) {
            transform.DOMove(to, duration);
        }

        public void fadeOut(int frame, int delay_frame = 0) {
            delay(delay_frame, () => { effect.fadeOut(frame); });
        }

        public void fadeIn(int frame, int delay_frame = 0) {
            delay(delay_frame, () => { effect.fadeIn(frame); });
        }

    }
}
