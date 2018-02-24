using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
    [CustomEditor(typeof(PoolObjectBase), true)]
    public class PoolObjectBaseEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            PoolObjectBase obj = target as PoolObjectBase;

            if ( GUILayout.Button("Set prefab") ) {
                obj.prefab = obj.gameObject;
            }
        }
    }
}
