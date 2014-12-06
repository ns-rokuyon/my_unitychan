using UnityEngine;
using System.Collections;

public class SimpleAI : AIController {
	private GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("unitychan");
	}

	// Update is called once per frame
	void Update () {
        float target_x = target.transform.position.x;
        float self_x = self.transform.position.x;

        if ( target_x < self_x ) {
            horizontal_input = -1.0f;
        }
        else {
            horizontal_input = +1.0f;
        }
        
	}
}
