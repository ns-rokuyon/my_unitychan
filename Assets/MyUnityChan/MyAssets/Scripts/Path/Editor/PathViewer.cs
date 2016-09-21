using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
    [CustomEditor(typeof(Path), true)]
    public class PathViewer : Editor {

        private Path path;

        void OnEnable() {
            path = target as Path;
        }

        void OnSceneGUI() {
            path.sceneGUI();
        }

        public override void OnInspectorGUI() {
            path.inspectorGUI();
            DrawDefaultInspector();
        }
    }
}
