using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
    [CustomEditor(typeof(Area))]
    public class AreaViewer : Editor {

        private Area area;

        void OnEnable() {
            area = target as Area;
        }

        public void OnSceneGUI() {
            area.sceneGUI();
        }

        public override void OnInspectorGUI() {
            area.inspectorGUI();
            DrawDefaultInspector();
        }
    }

    [CustomEditor(typeof(NormalArea))]
    public class NormalAreaViewer : AreaViewer { }

    [CustomEditor(typeof(SafeArea))]
    public class SafeAreaViewer : AreaViewer { }

    [CustomEditor(typeof(BossArea))]
    public class BossAreaViewer : AreaViewer { }
}
