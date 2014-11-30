using UnityEngine;
using System.Collections;

public class KickHitbox : AttackHitbox {
    Vector3 hitbox_pos_offset;  // from player.position

    public void create(Vector3 pos, Vector3 fw, AttackSpec atkspec) {
        transform.position = pos;
        spec = atkspec;
        this.time = spec.frame;
        end_timer = FrameCounter.startFrameCounter(time);

        forward = fw;
        hitbox_pos_offset = new Vector3(2.0f * fw.x, 4.5f, 0.0f);
        transform.position += hitbox_pos_offset;
    }

}
