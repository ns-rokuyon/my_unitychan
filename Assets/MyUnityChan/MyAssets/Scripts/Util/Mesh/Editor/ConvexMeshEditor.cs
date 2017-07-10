using UnityEngine;
using UnityEditor;
using System.Collections;

namespace MyUnityChan {
#if UNITY_EDITOR
    [ExecuteInEditMode]
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
                obj.GetComponent<MeshFilter>().mesh = mesh;
                obj.GetComponent<MeshCollider>().sharedMesh = mesh;
            }

            if ( GUILayout.Button("Reset mesh") ) {
                obj.GetComponent<MeshFilter>().mesh.Clear();
                obj.GetComponent<MeshCollider>().sharedMesh.Clear();
            }
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [CustomEditor(typeof(Area), true)]
    class AreaConvexMeshEditor : Editor {
        public override void OnInspectorGUI() {
            Area area = target as Area;
            base.OnInspectorGUI();

            if ( GUILayout.Button("Make a convex mesh") ) {
                if ( area.area_objects.Count == 0 ) {
                    Debug.LogWarning("No target object");
                    return;
                }

                area.gameObject.transform.localScale = area.area_objects[0].transform.localScale;
                area.gameObject.transform.localRotation = area.area_objects[0].transform.localRotation;
                area.gameObject.transform.position = area.area_objects[0].transform.position;

                // TODO: support multiple area_objects
                Mesh mesh = ConvexHull.createMesh(area.area_objects[0].GetComponent<MeshFilter>().sharedMesh.vertices);
                area.GetComponent<MeshFilter>().sharedMesh = mesh;

                if ( area.GetComponent<BoxCollider>() ) {
                    area.gameObject.removeComponent<BoxCollider>();
                }
                if ( area.GetComponent<MeshCollider>() ) {
                    area.gameObject.removeComponent<MeshCollider>();
                }

                // Add MeshCollider
                var meshcollider = area.gameObject.AddComponent<MeshCollider>();
                meshcollider.sharedMesh = mesh;
                meshcollider.convex = true;
                meshcollider.isTrigger = true;
            }

            if ( GUILayout.Button("Reset mesh") ) {
                // Create and attach cube mesh 
                area.GetComponent<MeshFilter>().sharedMesh.Clear();
                area.GetComponent<MeshFilter>().sharedMesh = 
                    GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().sharedMesh;

                if ( area.GetComponent<MeshCollider>() ) {
                    area.gameObject.removeComponent<MeshCollider>();
                }

                // Add BoxCollider
                var boxcollider = area.gameObject.AddComponent<BoxCollider>();
                boxcollider.isTrigger = true;
            }
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [CustomEditor(typeof(NormalArea))]
    class NormalAreaConvexMeshEditor : AreaConvexMeshEditor { }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [CustomEditor(typeof(BossArea))]
    class BossAreaConvexMeshEditor : AreaConvexMeshEditor { }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [CustomEditor(typeof(SafeArea))]
    class SafeAreaConvexMeshEditor : AreaConvexMeshEditor { }
#endif

}
