using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ConvexMesh))]
    class ConvexMeshEditor : Editor {
        public override void OnInspectorGUI() {
            ConvexMesh obj = target as ConvexMesh;
            base.OnInspectorGUI();

            if ( GUILayout.Button("Reset mesh") ) {
                obj.GetComponent<MeshFilter>().sharedMesh.Clear();
                obj.GetComponent<MeshCollider>().sharedMesh.Clear();
            }

            if ( GUILayout.Button("Make a convex mesh with target_objects") ) {
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
                obj.GetComponent<MeshCollider>().sharedMesh = mesh;
            }

            if ( GUILayout.Button("Make a convex mesh with Area.area_objects") ) {
                Area area = obj.GetComponent<Area>();
                if ( !area ) {
                    Debug.LogWarning("No Area components");
                    return;
                }
                if ( area.area_objects.Count == 0 ) {
                    Debug.LogWarning("No area objects");
                    return;
                }

                List<Vector3> vertices = new List<Vector3>();
                area.area_objects.ForEach(area_object => {
                    MeshFilter mf = area_object.GetComponent<MeshFilter>();
                    if ( mf ) {
                        vertices.AddRange(mf.sharedMesh.vertices.Select(v => v + area_object.transform.localPosition));
                        Debug.Log("Vertices of [" + area_object.name + "] were added (" +
                                  mf.sharedMesh.vertices.Length + ")");
                    }
                });
                Mesh mesh = ConvexHull.createMesh(vertices);
                obj.GetComponent<MeshFilter>().sharedMesh = mesh;
                obj.GetComponent<MeshCollider>().sharedMesh = mesh;
            }
        }
    }
#endif
}
