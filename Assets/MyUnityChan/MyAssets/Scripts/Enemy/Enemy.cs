using UnityEngine;
using System.Collections;

public class NPCharacter : Character {

}

public class Enemy : NPCharacter {
    public GameObject controller_prefab;

    protected void loadAttachedAI() {
        GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
        controller = controller_inst.GetComponent<Controller>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
