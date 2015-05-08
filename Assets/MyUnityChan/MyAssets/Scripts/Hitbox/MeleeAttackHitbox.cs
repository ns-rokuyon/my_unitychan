using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MeleeAttackHitbox : AttackHitbox {

        public override void ready(Vector3 pos, Vector3 fw, Vector3 offset, AttackSpec atkspec) {
            initPosition(pos + offset, fw, atkspec);
        }
    }
}
