using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyUnityChan {
    public class HpGauge : GUIObjectBase {

        private Slider slider;
        private Character character;

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

        public void setPosition(Vector3 pos) {
            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
