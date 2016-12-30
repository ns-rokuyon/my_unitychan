using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(Invincible))]
    public class CharacterStatus : Status {
        // values
        public bool freeze { get; set; }
        public virtual int hp { get; set; }

        // refarences to component
        public Invincible invincible { get; private set; }

        protected override void awake() {
            invincible = GetComponent<Invincible>();
            freeze = false;
            hp = 100;
        }

        protected override void start() {

        }

        protected override void update() {

        }
    }
}
