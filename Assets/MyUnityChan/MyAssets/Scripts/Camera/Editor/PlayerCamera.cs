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

            if ( GUILayout.Button("FadeOut") ) {
                cam.effect.fadeOut(60);
            }

            if ( GUILayout.Button("FadeIn") ) {
                cam.effect.fadeIn(60);
            }
        }
    }
}