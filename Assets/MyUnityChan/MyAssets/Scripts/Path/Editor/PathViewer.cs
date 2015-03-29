using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TwoPointsPath))]
public class PathViewer : Editor {

    private TwoPointsPath path;

    void OnEnable() {
        path = target as TwoPointsPath;
    }

    void OnSceneGUI() {
        Vector3[] endpoints = path.endpoint;
        Handles.color = Color.magenta;
        Handles.DrawLine(endpoints[0], endpoints[1]);
    }

    public override void OnInspectorGUI() {
        Vector3[] endpoints = path.GetComponent<TwoPointsPath>().endpoint;
        path.endpoint[0].z = path.gameObject.transform.position.z;
        path.endpoint[1].z = path.gameObject.transform.position.z;

        DrawDefaultInspector();
    }


}
