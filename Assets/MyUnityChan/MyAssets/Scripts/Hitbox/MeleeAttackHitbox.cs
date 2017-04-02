using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MeleeAttackHitbox : AttackHitbox {

        public override void ready(Vector3 pos, Vector3 fw, Vector3 offset, AttackSpec atkspec) {
            initPosition(pos + offset, fw, atkspec);
        }

        public void ready(Character ch, float offsetX, AttackSpec atkspec) {
            var fw = ch.getFrontVector();
            ready(ch.gameObject, fw, offsetX, atkspec);
        }

        public void ready(GameObject obj, Vector3 fw, float offsetX, AttackSpec atkspec) {
            initPosition(obj.transform.position.add(fw.x * offsetX, 0.0f, 0.0f), fw, atkspec);
        }
    }
}
