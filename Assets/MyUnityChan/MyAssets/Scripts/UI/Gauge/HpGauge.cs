using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyUnityChan {
    public class HpGauge : ObjectBase {
        public static string canvas_name = "Canvas";
        public static GameObject getCanvas() {
            return GameObject.Find(canvas_name);
        }

        Slider slider;
        Character character;

        void Awake() {
            slider = GetComponent<Slider>();
            character = null;
        }

        // Update is called once per frame
        void Update() {
            if ( character ) {
                slider.value = character.getHP();
            }
        }

        public void setCharacter(Character ch) {
            character = ch;
        }
    }
}
