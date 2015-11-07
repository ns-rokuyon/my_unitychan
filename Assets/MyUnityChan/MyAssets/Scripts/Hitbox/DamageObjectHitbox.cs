using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DamageObjectHitbox : AttackHitbox {
        private GameObject obj;

        protected override void UniqueUpdate() {
            if ( spec != null ) {
                if ( obj != null && obj.activeSelf ) {
                    transform.position = obj.transform.position;
                }
                else {
                    destroy();
                }
            }
        }

        public override void ready(GameObject _obj, AttackSpec atkspec) {
            obj = _obj;
            initPosition(atkspec);

            transform.position = obj.transform.position;
        }

        /*
        public void OnTriggerEnter(Collider other) {
            if ( other.tag == "Enemy" || other.tag == "Player" ) {
                Character character = ((Character)other.gameObject.GetComponent<Character>());
                spec.attack(character, this);
            }
        }
        */
    }
}
