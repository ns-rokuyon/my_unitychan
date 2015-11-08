using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class AttackSpec {
        [SerializeField] public int damage;
        [SerializeField] public int stun;      // time enemy is stuned
        [SerializeField] public int frame;      // time hitbox enabled
        [SerializeField] public string effect_name;

        public abstract void attack(Character character, Hitbox hitbox);
    }
}