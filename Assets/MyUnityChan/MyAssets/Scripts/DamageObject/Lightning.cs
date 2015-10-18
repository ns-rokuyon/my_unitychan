using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Lightning : DamageObjectBase {

        public class Spec : AttackSpec {
            public Spec() {
                damage = 10;
                frame = 9999;
            }

            public override void attack(Character character, Hitbox _hitbox) {
                GameObject owner = _hitbox.getOwner();
                if ( owner == null || owner == character.gameObject ) {
                    return;
                }
                character.damage(damage);
            }
        }

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {

        }

        public override void initialize() {
            throw new System.NotImplementedException();
        }

        public override void finalize() {
            throw new System.NotImplementedException();
        }
    }
}
