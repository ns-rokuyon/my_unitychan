using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

namespace MyUnityChan {
    [RequireComponent(typeof(ProCamera2D))]
    public class PlayerCameraProCamera2D : PlayerCamera {
        public ProCamera2D procam { get; protected set; }

        public override void Awake() {
            base.Awake();
            procam = GetComponent<ProCamera2D>();
        }

        public override void onChangeArea() {
            // TODO: fix z value
            camera_component.gameObject.transform.position = camera_component.gameObject.transform.position.changeZ(-14);
        }

        public override Vector3 getZoomPointOnChangeArea() {
            return player.transform.position.add(0, 1.0f,
                -Mathf.Abs(player.transform.position.z - transform.position.z / 2.0f));
        }

        public override void shake() {
            ProCamera2DShake.Instance.Shake(0);
        }
    }
}