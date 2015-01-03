﻿using UnityEngine;
using System.Collections;

public abstract class Action {
    // manage consistency of Update(), FixedUpdate(), and user inputs.
    // if flag is null, action is run by checking condition() only.
    public ActionFlag flag = new ActionFlag();      

    // define action name
    public abstract string name();
    public abstract bool condition();           // return whether or not the action manager calls perform()

    public virtual void perform() { }           // action method in Update()
	public virtual void performFixed() { }      // action method in FixedUpdate()

    public virtual void off_perform() { }               // action method in Update()
    public virtual void off_performFixed() { }          // action method in FixedUpdate()

    public virtual void constant_perform() { }          // action method in Update() constantly (update)
    public virtual void constant_performFixed() { }     // action method in FixedUpdate() constantly 

    public virtual void prepare() { }
    public virtual void end() { }
    public virtual void effect() { }
}

public class ActionFlag {
    public bool condition = false;
    public bool ready_to_update = false;
    public bool ready_to_fixedupdate = false;

    public void setAll() {
        condition = true;
        ready_to_update = true;
        ready_to_fixedupdate = true;
    }

    public void resetAll() {
        condition = false;
        ready_to_update = false;
        ready_to_fixedupdate = false;
    }

    public void doneUpdate() { 
        ready_to_update = false;
        if ( !ready_to_fixedupdate ) {
            //condition = false;
            done();
        }
    }
    public void doneFixedUpdate() { 
        ready_to_fixedupdate = false;
        if ( !ready_to_update ) {
            //condition = false;
            done();
        }
    }
    public void done() {
        if ( ready_to_update || ready_to_fixedupdate ) {
            Debug.LogError("update or fixedupdate is not done");
        }
        condition = false;
    }
}