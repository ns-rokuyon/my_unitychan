using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [RequireComponent(typeof(Invincible))]
    public class CharacterStatus : Status {
        [SerializeField]
        public CharacterParameter parameters;

        public int ATK { get { return parameters.atk; } }
        public int DEF { get { return parameters.def; } }
        public int MATK { get { return parameters.matk; } }
        public int MDEF { get { return parameters.mdef; } }

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
