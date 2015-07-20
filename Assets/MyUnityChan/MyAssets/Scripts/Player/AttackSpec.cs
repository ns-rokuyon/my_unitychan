using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class AttackSpec {
        public int damage { get; set; }
        public int stun { get; set; }       // time enemy is stuned
        public int frame { get; set; }      // time hitbox enabled
        public string effect_name { get; set; }
        public abstract void attack(Character character, Hitbox hitbox);
    }
}