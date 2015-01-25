using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class CharacterStatus : Status {
        // values
        public int hp { get; set; }

        // prefabs
        public GameObject invincible_prefab;

        // refarences to component
        public Invincible invincible { get; private set; }

        protected override void awake() {
            invincible = (Instantiate(invincible_prefab) as GameObject).GetComponent<Invincible>();
            hp = 100;
        }

        protected override void start() {

        }

        protected override void update() {

        }
    }
}
