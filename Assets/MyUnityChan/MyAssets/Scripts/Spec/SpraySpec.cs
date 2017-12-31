using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class SpraySpec : AttackSpec {
        public override int regularizeDamage(int base_damage, Character character) {
            return base_damage + character.status.MATK;
        }
    }
}