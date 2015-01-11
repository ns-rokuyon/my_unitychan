using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PunchHitbox : AttackHitbox {

        public void create(Vector3 pos, Vector3 fw, AttackSpec atkspec) {
            initPosition(pos, fw, atkspec);

            Vector3 hitbox_pos_offset = new Vector3(2.0f * fw.x, 4.5f, 0.0f);
            transform.position += hitbox_pos_offset;
        }


    }
}
