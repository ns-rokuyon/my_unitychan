using UnityEngine;
using System.Collections;

public class AttackHitbox : Hitbox {
    protected AttackSpec spec;

    public void OnTriggerEnter(Collider other) {
        if ( other.tag == "Enemy" ) {
            Enemy enemy = ((Enemy)other.gameObject.GetComponent<Enemy>());
            spec.attack(enemy, this);
        }
    }
}

