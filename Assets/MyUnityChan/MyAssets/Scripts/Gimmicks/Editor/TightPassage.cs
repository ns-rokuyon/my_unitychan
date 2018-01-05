using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(TightPassage))]
    public class TightPassageEditor : Editor {
        public override void OnInspectorGUI() {
            TightPassage parent = target as TightPassage;
            base.OnInspectorGUI();

            if ( GUILayout.Button("Check") ) {
                MeshCollider[] colliders = parent.GetComponentsInChildren<MeshCollider>();
                if ( colliders.Length == 0 ) {
                    Debug.LogWarning("No colliders");
                    return;
                }

                colliders.ToList().ForEach(mc => {
                    Debug.Log("vertexCount = " + mc.sharedMesh.vertexCount);
                });
            }
        }
    }
#endif
}
