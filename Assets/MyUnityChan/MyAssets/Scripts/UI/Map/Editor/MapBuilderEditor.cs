using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MyUnityChan {
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
                    if ( area is SystemArea )
                        continue;

                    string name = area.gameObject.name;
                    GameObject obj = PrefabInstantiater.create("Prefabs/UI/Map/MapAreaElement", builder.build_to);
                    obj.name = name + "__map";
                    obj.AddComponent<MapAreaElement>();
                    obj.layer = builder.layer();
                    Mesh mesh = null;

                    // Copy mesh to area element from area
                    mf = area.gameObject.GetComponent<MeshFilter>();
                    mesh = mf.sharedMesh;
                    obj.GetComponent<MeshFilter>().sharedMesh = mesh;

                    // Apply material
                    obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/UI/Map/MapArea");

                    // Position
                    obj.gameObject.transform.position = area.gameObject.transform.position;
                    obj.transform.localScale = area.gameObject.transform.localScale;
                    obj.transform.localRotation = area.gameObject.transform.localRotation;

                    // Set area ref
                    obj.GetComponent<MapAreaElement>().area_path = area.gameObject.getHierarchyPath();
                }

                Dictionary<GameObject, GameObject> gate_connectionpoint_map = new Dictionary<GameObject, GameObject>(); // gate object -> map connection object
                AreaGate[] gates = FindObjectsOfType<AreaGate>();
                foreach ( AreaGate gate in gates ) {
                    // Create area joint point into mapviewer
                    GameObject gatepoint = PrefabInstantiater.create(Const.Prefab.UI["MAP_CONNECTION_POINT"], builder.build_to);
                    gatepoint.AddComponent<MapConnectionPointElement>();
                    gatepoint.layer = builder.layer();
                    gatepoint.transform.position = gate.gameObject.transform.position;
                    gatepoint.GetComponent<MapConnectionPointElement>().gate_path = gate.gameObject.getHierarchyPath();
                    gate_connectionpoint_map.Add(gate.gameObject, gatepoint);
                }

                // Scaling
                builder.gameObject.transform.localScale = builder.scale;

                // Build connection for each point
                foreach ( AreaGate gate in gates ) {
                    GameObject t = gate_connectionpoint_map[gate.gameObject];
                    GameObject pair = gate_connectionpoint_map[gate.gate_pair.gameObject];
                    t.GetComponent<MapConnectionPointElement>().pair = pair;
                }
            }

            if ( GUILayout.Button("Clear") ) {
                builder.gameObject.transform.localScale = Vector3.one;
                var area_elements = FindObjectsOfType<MapElement>();
                foreach ( var obj in area_elements ) {
                    DestroyImmediate(obj.gameObject);
                }
            }
        }
    }
#endif

}
