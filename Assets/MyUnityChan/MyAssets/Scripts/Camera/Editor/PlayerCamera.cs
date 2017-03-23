using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
    [CustomEditor(typeof(PlayerCamera), true)]
    public class PlayerCameraEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            PlayerCamera cam = target as PlayerCamera;

            if ( GUILayout.Button("Centralize") ) {
                cam.centralize();
            }
        }
    }
}