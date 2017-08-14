using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyUnityChan {
    [CustomEditor(typeof(ActionManager), true)]
    public class ActionManagerEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            ActionManager manager = target as ActionManager;

            if ( GUILayout.Button("Show actions") ) {
                DebugManager.log("Show actions -------------");
                manager.getAllActionKeys().ForEach(name => {
                    DebugManager.log("    - " + name);
                });
                DebugManager.log("-------------");
            }
        }
    }
}
