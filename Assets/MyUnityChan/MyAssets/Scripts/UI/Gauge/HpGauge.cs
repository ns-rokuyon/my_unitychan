using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyUnityChan {
    public class HpGauge : GUIObjectBase {
        public bool use_energy_bar_toolkit;

        protected EnergyBar energybar;
        protected Slider slider;
        protected Character character;

        void Awake() {
            if ( use_energy_bar_toolkit ) 
                energybar = GetComponent<EnergyBar>();
            else 
                slider = GetComponent<Slider>();

            character = null;
        }

        // Update is called once per frame
        void Update() {
            if ( character ) {
                if ( use_energy_bar_toolkit ) 
                    energybar.SetValueCurrent(character.getHP());
                else 
                    slider.value = character.getHP();
            }
        }

        public virtual void setCharacter(Character ch) {
            character = ch;
        }

        public void setMapHp(int maxhp) {
            if ( use_energy_bar_toolkit )
                energybar.SetValueMax(maxhp);
            else
                slider.maxValue = maxhp;
        }

        public void setPosition(Vector3 pos) {
            GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
