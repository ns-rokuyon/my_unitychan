using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(EnergyBar))]
    public class ReservedHpGauge : HpGauge {
        void Awake() {
            energybar = GetComponent<EnergyBar>();
            character = null;
        }

        void Update() {
            if ( character ) energybar.SetValueCurrent(character.getReservedHP());
        }

        public override void setCharacter(Character ch) {
            base.setCharacter(ch);
            energybar.SetValueMax(
                Const.Max.ENERGY_TANKS * Const.Unit.RESERVED_HP
            );
        }
    }
}