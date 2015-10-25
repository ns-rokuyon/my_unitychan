using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
    [CustomEditor(typeof(Area), true)]
    public class AreaViewer : Editor {

        private Area area;

        void OnEnable() {
            area = target as Area;
        }

        void OnSceneGUI() {
            area.sceneGUI();
        }

        public override void OnInspectorGUI() {
            area.inspectorGUI();
            DrawDefaultInspector();
        }


    }
}
