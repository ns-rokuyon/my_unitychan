using UnityEngine;
using System.Collections;

public class Character : ObjectBase {
    // prefabs
    public GameObject status_prefab;

    // references to component
	protected Controller controller;

    // vars
    protected FrameTimerState inputlock_timer;

	public Controller getController(){
		return controller;
	}

    public virtual bool isTouchedWall() {
        // check character is in front of wall
        CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
        return Physics.Raycast(transform.position + new Vector3(0, capsule_collider.bounds.size.y, 0), transform.forward, 2.0f) ||
            Physics.Raycast(capsule_collider.bounds.center, transform.forward, 2.0f);
    }

    public void lockInput(int frame) {
        // disable movement by inputs for N frames specified
        inputlock_timer.createTimer(frame);
    }

    public bool isInputLocked() {
        // return true when inputs are locked
        return inputlock_timer.isRunning();
    }

}
