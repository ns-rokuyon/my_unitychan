using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class PlayerCameraPosition {
        [SerializeField]
        public bool on;

        [SerializeField]
        public Vector3 position_diff;

        [SerializeField]
        public Vector3 rotation;

        static public PlayerCameraPosition getDefault() {
            PlayerCameraPosition pos = new PlayerCameraPosition();
            pos.on = true;
            pos.position_diff = new Vector3(0.0f, 1.5f, -5.0f);
            pos.rotation = new Vector3(10.0f, 0.0f, 0.0f);

            return pos;
        }
    }
}
