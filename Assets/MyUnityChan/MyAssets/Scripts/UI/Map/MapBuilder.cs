using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MyUnityChan {
    public class MapBuilder : SingletonObjectBase<MapBuilder> {

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {

        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(MapBuilder))]
    public class MapBuilderEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            MapBuilder builder = target as MapBuilder;

            if ( GUILayout.Button("Build") ) {
                Debug.Log("Build");

                MeshFilter mf = null;
                Area[] found = FindObjectsOfType<Area>();
                foreach ( Area area in found ) {
                    string name = area.gameObject.name;
                    GameObject obj = PrefabInstantiater.create("Prefabs/UI/Map/MapAreaElement", "Map");
                    obj.name = name + "__map";
                    Mesh mesh = null;

                    // Copy mesh to area element from area
                    mf = area.gameObject.GetComponent<MeshFilter>();
                    mesh = mf.sharedMesh;
                    obj.GetComponent<MeshFilter>().sharedMesh = mesh;

                    // Apply material
                    obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Common/Glass");

                    // Position
                    obj.gameObject.transform.position = area.gameObject.transform.position;
                    obj.transform.localScale = area.gameObject.transform.localScale;
                    obj.transform.localRotation = area.gameObject.transform.localRotation;
                }

                AreaGate[] gates = FindObjectsOfType<AreaGate>();
                foreach ( AreaGate gate in gates ) {
                    // Create area joint point into mapviewer
                    GameObject gatepoint = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], "Map");
                    gatepoint.transform.position = gate.gameObject.transform.position;
                }
                    
                // Scaling
                builder.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            if ( GUILayout.Button("Clear") ) {
                builder.gameObject.transform.localScale = Vector3.one;
                var area_elements = FindObjectsOfType<ConvexMesh>();
                foreach ( var obj in area_elements ) {
                    DestroyImmediate(obj.gameObject);
                }
            }
        }
    }
#endif
}