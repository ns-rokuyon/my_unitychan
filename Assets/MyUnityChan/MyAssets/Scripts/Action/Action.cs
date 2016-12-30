using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Action : StructBase {
        // manage consistency of Update(), FixedUpdate(), and user inputs.
        // if flag is null, action is run by checking condition() only.
        public ActionFlag flag = new ActionFlag();

        public bool activation = true;

        public int priority { get; protected set; }
        public bool skip_lower_priority { get; protected set; }

        // define action name
        public abstract string name();
        public abstract bool condition();           // return whether or not the action manager calls perform()

        public virtual void perform() { }           // action method in Update()
        public virtual void performFixed() { }      // action method in FixedUpdate()

        public virtual void off_perform() { }               // action method in Update()
        public virtual void off_performFixed() { }          // action method in FixedUpdate()

        public virtual void constant_perform() { }          // action method in Update() constantly (update)
        public virtual void constant_performFixed() { }     // action method in FixedUpdate() constantly 

        public virtual void performLate() { }       // action method in LateUpdate()

        public virtual void prepare() { }
        public virtual void end() { }
        public virtual void effect() { }

        public void enable() {
            activation = true;
        }

        public void disable() {
            activation = false;
        }
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
}