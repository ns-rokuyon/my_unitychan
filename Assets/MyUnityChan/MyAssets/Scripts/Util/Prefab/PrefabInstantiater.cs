using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PrefabInstantiater : SingletonObjectBase<PrefabInstantiater> {
        // World object
        //=================================================================================================
        public static GameObject create(string resource_path, string parent = null) {
            GameObject obj = (Instantiate(Resources.Load(resource_path)) as GameObject);
            if ( parent != null ) {
                obj.setParent(parent);
            }
            return obj;
        }

        public static GameObject create(string resource_path, GameObject parent) {
            GameObject obj = (Instantiate(Resources.Load(resource_path)) as GameObject);
            obj.setParent(parent);
            return obj;
        }

        public static T createAndGetComponent<T>(string resource_path, string parent = null) {
            return create(resource_path, parent).GetComponent<T>();
        }

        public static T createAndGetComponent<T>(string resource_path, GameObject parent) {
            return create(resource_path, parent).GetComponent<T>();
        }

        // UI
        //=================================================================================================
        public static GameObject createUI(string resource_path, GameObject parent) {
            RectTransform rt = createAndGetComponent<RectTransform>(resource_path, parent);
            rt.localScale = new Vector3(1, 1, 1);
            rt.localPosition = rt.localPosition.changeZ(0.0f);
            return rt.gameObject;
        }

        public static T createUIAndGetComponent<T>(string resource_path, GameObject parent) {
            return createUI(resource_path, parent).GetComponent<T>();
        }

        // Worldspace UI
        //=================================================================================================
        public static GameObject createWorldUI(string resource_path) {
            return createWorldUI(resource_path, Vector3.one);
        }

        public static GameObject createWorldUI(string resource_path, Vector3 scale) {
            GameObject canvas = GUIObjectBase.getCanvas("Canvas_WorldSpace");
            RectTransform rt = createAndGetComponent<RectTransform>(resource_path, canvas);
            rt.localScale = scale;
            rt.localPosition = rt.localPosition.changeZ(0.0f);
            return rt.gameObject;
        }

        public static T createWorldUIAndGetComponent<T>(string resource_path) {
            return createWorldUI(resource_path).GetComponent<T>();
        }

        public static T createWorldUIAndGetComponent<T>(string resource_path, Vector3 scale) {
            return createWorldUI(resource_path, scale).GetComponent<T>();
        }
    }
}
