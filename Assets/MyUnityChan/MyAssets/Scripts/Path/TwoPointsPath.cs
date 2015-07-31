using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace MyUnityChan {
    [ExecuteInEditMode]
    public class TwoPointsPath : Path {
        public Vector3 left_point;
        public Vector3 right_point;

        // Use this for initialization
        void Start () {
        
        }
        
        // Update is called once per frame
        void Update () {
        
        }

    #if UNITY_EDITOR
        public override void sceneGUI() {
            Handles.color = Color.magenta;
            Vector3[] points = new Vector3[] { left_point, right_point };
            Handles.DrawAAPolyLine(10, points);
            left_point = Handles.PositionHandle(left_point, Quaternion.identity);
            right_point = Handles.PositionHandle(right_point, Quaternion.identity);
        }
    #else
        public override void sceneGUI() {
        }
    #endif

        public override void inspectorGUI() {
            // freeze position z
            left_point.z = gameObject.transform.position.z;
            right_point.z = gameObject.transform.position.z;

            if ( left_point.x > right_point.x ) {
                float tmpx = right_point.x;
                right_point.x = left_point.x;
                left_point.x = tmpx;
            }
        }
    }
}