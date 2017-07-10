using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MyUnityChan {
    public static class UIHelper {

        public enum LayoutDirection {
            VERTICAL,
            HORIZONTAL
        }

        public static string getCurrentSelectedUIObjectName() {
            var obj = EventSystem.current.currentSelectedGameObject;
            if ( !obj ) {
                return "No selected ui objects";
            }
            return obj.name;
        }

        public static void makeExplicitNavigation<T>(List<T> selectables, LayoutDirection direction) where T : Selectable {
            for ( int i = 0; i < selectables.Count; i++ ) {
                var next = i + 1 >= selectables.Count ? selectables[0] : selectables[i + 1];
                var prev = i - 1 < 0 ? selectables[selectables.Count - 1] : selectables[i - 1];
                var navigation = selectables[i].navigation;

                if ( direction == LayoutDirection.VERTICAL ) {
                    navigation.selectOnUp = prev;
                    navigation.selectOnDown = next;
                }
                else {
                    navigation.selectOnLeft = prev;
                    navigation.selectOnRight = next;
                }

                selectables[i].navigation = navigation;
            }
        }
    }
}