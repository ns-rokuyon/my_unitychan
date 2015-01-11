using UnityEngine;
using System.Collections.Generic;

public abstract class Controller : ObjectBase {
	public enum Movement {
		JUMP = 0,
		SLIDING,
		ATTACK,
		PROJECTILE,
        DASH,
		len
	};

    protected Character self;

	protected List<bool> inputs;
	protected float horizontal_input;
	protected float vertical_input;

	// Use this for initialization
	void Awake(){
		inputs = new List<bool>();
		for (int i=0; i<(int)Movement.len; i++) {
			inputs.Add(false);
		}
		horizontal_input = 0.0f;
		vertical_input = 0.0f;
	}
	
	// Update is called once per frame
	void Update(){
	}


    public void setSelf(Character ch) {
        self = ch;
    }

    protected void clearAllInputs() {
		for (int i=0; i<inputs.Count; i++) {
            inputs[i] = false;
		}
    }

	public bool keyJump(){ return inputs[(int)Movement.JUMP];}
	public bool keySliding(){ return inputs[(int)Movement.SLIDING];}
	public bool keyAttack(){ return inputs[(int)Movement.ATTACK];}
	public bool keyProjectile(){ return inputs[(int)Movement.PROJECTILE];}
    public bool keyDash() { return inputs[(int)Movement.DASH]; }
	public float keyHorizontal(){ return horizontal_input;}
	public float keyVertical(){ return vertical_input; }

}

public abstract class PlayerController : Controller {
}

public abstract class AIController : Controller {
}

