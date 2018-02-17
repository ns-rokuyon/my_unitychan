using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MyUnityChan {
    /* 
    EnumLabelAttribute
    http://tangerineboxgames.blogspot.jp/2016/05/unityeditorinspectorenum.html
    */

    [CustomPropertyDrawer(typeof(EnumLabelAttribute))]
    public class EnumLabelDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var attr = attribute as EnumLabelAttribute;
            var match = new System.Text.RegularExpressions.Regex(@"Element (?<i>\d+)").Match(label.text);

            if ( !match.Success ) {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            int i = int.Parse(match.Groups["i"].Value);

            if ( i < attr.labels.Count ) {
                EditorGUI.PropertyField(position, property,
                                        new GUIContent(attr.labels[i]), true);

                property.Next(true);
                property.enumValueIndex = i;
            }
            else
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}