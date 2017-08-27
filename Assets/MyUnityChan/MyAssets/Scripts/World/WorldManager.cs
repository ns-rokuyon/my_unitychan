using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class WorldManager : SingletonObjectBase<WorldManager> {
        public Light sun;

        public LightParameter default_sun_setting { get; set; }

        void Awake() {
            addTagToGroundObjects();
            if ( assertLight() ) {
                default_sun_setting = new LightParameter(sun);
            }
        }

        public bool assertLight() {
            if ( !sun ) {
                DebugManager.warn("WorldManager doesn't have a reference to Directional light");
                return false;
            }
            return true;
        }

        public static void setIntensity(float value) {
            if ( !Instance.assertLight() )
                return;
            Instance.sun.intensity = value;
        }

        public static void setDefault() {
            Instance.default_sun_setting.apply(Instance.sun);
        }

        private void addTagToGroundObjects() {
            var objs = GetComponentsInChildren<Collider>()
                .Where(col => !col.isTrigger)
                .Select(col => col.gameObject)
                .Where(obj => obj.tag == "Untagged");
            foreach ( GameObject o in objs ) {
                o.tag = "Ground";
            }
        }
    }

    [System.Serializable]
    public class LightParameter {
        [SerializeField]
        public float intensity;

        [SerializeField]
        public float indirect_multiplier;

        [SerializeField]
        public Color color;

        public LightParameter(float _intensity, float _indirect_multiplier, Color _color) {
            intensity = _intensity;
            indirect_multiplier = _indirect_multiplier;
            color = _color;
        }

        public LightParameter(Light l) {
            intensity = l.intensity;
            indirect_multiplier = l.bounceIntensity;
            color = l.color;
        }

        public void apply(Light l) {
            l.intensity = intensity;
            l.bounceIntensity = indirect_multiplier;
            l.color = color;
        }
    }
}