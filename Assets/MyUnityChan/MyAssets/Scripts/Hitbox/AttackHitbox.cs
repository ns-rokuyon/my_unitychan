using UnityEngine;
using System.Collections;

public class AttackHitbox : Hitbox {
    protected AttackSpec spec = null;

    protected void initPosition(AttackSpec atkspec) {
        // attack parameter
        spec = atkspec;

        // timer
        this.time = spec.frame;
        end_timer = FrameCounter.startFrameCounter(time);
    }

    protected void initPosition(Vector3 pos, Vector3 fw, AttackSpec atkspec) {
        // position
        transform.position = pos;
        forward = fw;

        // attack parameter
        spec = atkspec;

        // timer
        this.time = spec.frame;
        end_timer = FrameCounter.startFrameCounter(time);
    }

    public void OnTriggerEnter(Collider other) {
        if ( other.tag == "Enemy" ) {
            Enemy enemy = ((Enemy)other.gameObject.GetComponent<Enemy>());
            spec.attack(enemy, this);
        }
    }
}

