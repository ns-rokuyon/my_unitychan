using UnityEngine;
using System.Collections;

public class Hitbox : ObjectBase {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            Debug.Log("hit");
        }
    }
}
