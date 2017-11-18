using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class DropItem : Item {
        public Attractable attractable { get; protected set; }

        public override void setup() {
            attractable = GetComponent<Attractable>();
            if ( attractable ) {
                attractable.target = GameStateManager.getPlayer().transform;
                attractable.go();
            }
        }

        public override void destroy(Player player) {
            if ( attractable ) {
                attractable.clear();
            }
        }

        public override void perform(Player player) {
        }

        public override void initialize() {
        }

        public override void finalize() {
        }
    }
}
