using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
    [CustomEditor(typeof(ObjectBase), true)]
    public class ObjectBaseEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            ObjectBase ob = target as ObjectBase;

            if ( GUILayout.Button("Show data") ) {
                string p = string.Format(@"Data ( {0}::{1} )
===========================================================
  * Position : {2}
  * Area : {3}
", ob.name, target.GetType().Name, ob.transform.position, ob.parent_area ? ob.parent_area.name : "Undefined");
                DebugManager.log(p);
            }
        }
    }
}
