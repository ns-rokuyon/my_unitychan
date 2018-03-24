using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(PointingLineBuildingBlock), true)]
    class PointingLineEditor : Editor {
        public override void OnInspectorGUI() {
            PointingLineBuildingBlock bb = target as PointingLineBuildingBlock;
            base.OnInspectorGUI();

            if ( GUILayout.Button("Open") ) {
                bb.open();
            }

            if ( GUILayout.Button("Close") ) {
                bb.close();
            }

            if ( GUILayout.Button("Terminate") ) {
                bb.terminate();
            }
        }
    }

    [ExecuteInEditMode]
    public static class PointingLineCreatorEditor {
        public static readonly string root_path = "Assets/MyAssets/Resources/";

        [MenuItem("GameObject/PointingLineBuildingBlock/PointingLine", false, 10)]
        public static void createPointingLine(MenuCommand mc) {
            string prefab_path = root_path + Const.Prefab.PointingLineBuildingBlock[Const.ID.PointingLineBuildingBlock.LINE];
            create(prefab_path, mc);
        }

        [MenuItem("GameObject/PointingLineBuildingBlock/PointingLineSequence", false, 10)]
        public static void createPointingLineSequence(MenuCommand mc) {
            string prefab_path = root_path + Const.Prefab.PointingLineBuildingBlock[Const.ID.PointingLineBuildingBlock.SEQUENCE];
            create(prefab_path, mc);
        }

        public static void create(string prefab_path, MenuCommand mc = null) {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefab_path + ".prefab", typeof(GameObject));

            if ( !prefab ) {
                Debug.LogError(prefab_path + " was not found");
                return;
            }
            GameObject go = (GameObject) PrefabUtility.InstantiatePrefab(prefab);

            if ( mc != null && mc.context ) {
                GameObject context = mc.context as GameObject;
                GameObjectUtility.SetParentAndAlign(go, context);

                var sequence = context.GetComponent<PointingLineSequence>();
                if ( sequence ) {
                    var building_block = go.GetComponent<PointingLineBuildingBlock>();
                    sequence.append(building_block);
                    EditorUtility.SetDirty(sequence);
                }
            }
            else if ( Selection.activeObject is GameObject ) {
                GameObjectUtility.SetParentAndAlign(go, Selection.activeObject as GameObject);
            }

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
#endif
}
