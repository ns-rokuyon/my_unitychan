using UnityEngine;
using System.Collections;

public class NPCharacter : Character {

}

public class Enemy : NPCharacter {
    public GameObject controller_prefab;
    protected int stunned = 0;

    protected void loadAttachedAI() {
        GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
        controller = controller_inst.GetComponent<Controller>();

        ((AIController)controller).setSelf(this);
    }

    public void stun(int stun_power) {
        stunned = stun_power;
    }

    public bool isStunned() {
        return stunned > 0 ? true : false;
    }

    protected void updateStunned() {
        if ( stunned > 0 ) {
            stunned--;
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
