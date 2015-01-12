using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class KickHitbox : AttackHitbox {

        public void create(Vector3 pos, Vector3 fw, AttackSpec atkspec) {
            initPosition(pos, fw, atkspec);

            Vector3 hitbox_pos_offset = new Vector3(0.4f * fw.x, 0.8f, 0.0f);
            transform.position += hitbox_pos_offset;
        }

    }
}
