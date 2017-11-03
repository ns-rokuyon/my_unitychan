using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DamageObjectHitbox : AttackHitbox {
        private GameObject obj;

        protected override void UniqueUpdate() {
            if ( depend_on_parent_object )
                return;

            if ( spec != null ) {
                if ( obj != null && obj.activeSelf ) {
                    transform.position = obj.transform.position;
                }
                else {
                    destroy();
                }
            }
        }

        public override void ready(GameObject _obj, AttackSpec atkspec, bool keep_position = false) {
            obj = _obj;
            initPosition(atkspec);

            if ( keep_position )
                return;

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
