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

        public PlayerCameraPosition(Vector3 _posdiff, Vector3 _rot) {
            on = true;
            position_diff = _posdiff;
            rotation = _rot;
        }

        static public PlayerCameraPosition getDefault() {
            PlayerCameraPosition pos = new PlayerCameraPosition(
                new Vector3(0.0f, 1.5f, -5.0f), new Vector3(10.0f, 0.0f, 0.0f) );
            return pos;
        }

        static public PlayerCameraPosition operator +(PlayerCameraPosition a, PlayerCameraPosition b) {
            return new PlayerCameraPosition(
                a.position_diff + b.position_diff,
                a.rotation + b.rotation
                );
        }

    }
}
