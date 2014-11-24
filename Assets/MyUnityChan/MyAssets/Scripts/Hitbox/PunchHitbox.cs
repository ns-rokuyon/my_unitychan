using UnityEngine;
using System.Collections;

public class PunchHitbox : AttackHitbox {
    FrameCounter start_timer = null;
    FrameCounter end_timer = null;
    int time = 0;
    Vector3 hitbox_pos_offset;  // from player.position
    Vector3 forward;

    public void create(Vector3 pos, Vector3 fw, int start_offset, int time) {
        transform.position = pos;
        this.time = time;
        if ( start_offset == 0 ) {
            end_timer = FrameCounter.startFrameCounter(time);
        }
        else {
            start_timer = FrameCounter.startFrameCounter(start_offset);
        }

        forward = fw;
        hitbox_pos_offset = new Vector3(2.0f * fw.x, 4.5f, 0.0f);
        transform.position += hitbox_pos_offset;
    }

    void Update() {
        if ( end_timer == null ) {
            if ( start_timer != null && start_timer.finished() ) {
                end_timer = FrameCounter.startFrameCounter(time);
            }
        }
        else if ( end_timer.finished() ){
            Destroy(this.gameObject);
            return;
        }

        if ( start_timer != null ) {
            start_timer.update();
        }
        if ( end_timer != null ) {
            end_timer.update();
        }
    }

    public void OnTriggerEnter(Collider other) {
        if ( other.tag == "Enemy" ) {
            // TODO
            ((Enemy)other.gameObject.GetComponent<Enemy>()).stun(60);
            other.rigidbody.AddForce(new Vector3(forward.x * 20.0f, 5.0f, 0.0f), ForceMode.Impulse);
        }
    }
}
