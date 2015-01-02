using UnityEngine;
using System.Collections;

public abstract class Action {

    public abstract string name();      // define action name

	public abstract void perform();

    public virtual void perform_off() {
    }

	public virtual bool condition(){
		return true;
	}

    public virtual bool prepare() {
        return true;
    }

    public virtual bool end() {
        return true;
    }

    public virtual bool update() {
        return true;
    }

    public virtual bool effect() {
        return true;
    }
}
