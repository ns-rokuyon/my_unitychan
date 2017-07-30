using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

namespace MyUnityChan {
    public abstract class Action : StructBase {
        // manage consistency of Update(), FixedUpdate(), and user inputs.
        // if flag is null, action is run by checking condition() only.
        public ActionFlag flag = new ActionFlag();

        public bool activation = true;

        public bool initialized { get; set; }
        public int priority { get; protected set; }
        public bool skip_lower_priority { get; protected set; }
        public bool keep_skipping_lower_priority_in_transaction { get; protected set; }
        public List<System.Action> perform_callbacks { get; set; }

        public int transaction_frame_count { get; set; }
        public virtual bool use_transaction { get; set; }
        public virtual IDisposable transaction { get; set; }

        public abstract Character owner { get; }

        // define action name
        public abstract string name();
        public abstract bool condition();           // return whether or not the action manager calls perform()

        public virtual void perform() { }           // action method in Update()
        public virtual void performFixed() { }      // action method in FixedUpdate()
        public virtual void performLate() { }       // action method in LateUpdate()

        public virtual void off_perform() { }               // action method in Update()
        public virtual void off_performFixed() { }          // action method in FixedUpdate()

        public virtual void constant_perform() { }          // action method in Update() constantly (update)
        public virtual void constant_performFixed() { }     // action method in FixedUpdate() constantly 
        public virtual void constant_performLate() { }      // action method in LateUpdate() constantly

        public virtual void init() { }      // action method in Start()

        public virtual void prepare() { }
        public virtual void end() { }
        public virtual void effect() { }

        public void enable() {
            activation = true;
        }

        public void disable() {
            activation = false;
        }

        public bool isFreeTransaction() {
            if ( !use_transaction ) {
                DebugManager.warn("use_transaction flag is false, but isFreeTransaction() method was called");
                return true;
            }
            return transaction == null;
        }

        public void beginTransaction(int frame) {
            if ( transaction != null )
                return;
            DebugManager.log("beginTransaction = " + frame);
            transaction = Observable.TimerFrame(0, 1)
                .Take(frame)
                .Subscribe(f => {
                    transaction_frame_count = (int)f;
                }, () => {
                    transaction = null;
                    DebugManager.log("endTransaction = " + frame);
                })
                .AddTo(owner);
        }
    }

    public class ActionFlag {
        public bool condition { get; protected set; }
        public bool ready_to_update { get; protected set; }
        public bool ready_to_fixedupdate { get; protected set; }

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