using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
    public class ConvexMesh : ObjectBase {
        public GameObject target_object;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [CustomEditor(typeof(ConvexMesh))]
    class ConvexMeshEditor : Editor {
        public override void OnInspectorGUI() {
            ConvexMesh obj = target as ConvexMesh;
            base.OnInspectorGUI();

            if ( GUILayout.Button("Make") ) {
                if ( !obj.target_object ) {
                    Debug.LogWarning("No target object");
                    return;
                }

                MeshFilter target_meshfilter = obj.target_object.GetComponent<MeshFilter>();
                if ( !target_meshfilter ) {
                    Debug.LogWarning("No MeshFilter in target object");
                    return;
                }

                Mesh mesh = ConvexHull.createMesh(target_meshfilter.sharedMesh.vertices);
                obj.GetComponent<MeshFilter>().sharedMesh = mesh;
            }

            if ( GUILayout.Button("Reset mesh") ) {
                obj.GetComponent<MeshFilter>().sharedMesh.Clear();
            }
        }


    }
#endif

}
