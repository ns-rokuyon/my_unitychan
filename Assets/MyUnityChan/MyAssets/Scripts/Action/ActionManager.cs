using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class ActionManager : ObjectBase {
        protected Character character;
        protected Dictionary<string, Action> actions;

        protected abstract void start();
        protected abstract void update();

        void Awake() {
            character = null;
            actions = new Dictionary<string, Action>();
        }

        void Start() {
            start();
        }

        void Update() {
            foreach ( KeyValuePair<string, Action> pair in actions ) {
                Action action = pair.Value;

                // call action methods in Update() constantly
                action.constant_perform();

                if ( action.flag == null ) {
                    if ( action.condition() ) {
                        action.perform();
                    }
                    else {
                        action.off_perform();
                    }
                    continue;
                }

                if ( action.flag.condition == false && action.condition() ) {
                    // action start point
                    action.flag.setAll();

                    // call action methods in Update()
                    action.perform();

                    // ready_to_update flag : true -> false
                    action.flag.doneUpdate();
                }
                else {
                    action.off_perform();
                }
            }
        }

        void FixedUpdate() {
            update();

            foreach ( KeyValuePair<string, Action> pair in actions ) {
                Action action = pair.Value;

                // call action methods in Update() constantly
                action.constant_performFixed();

                if ( action.flag == null ) {
                    if ( action.condition() ) {
                        // call action methods in FixedUpdate()
                        act(action);
                    }
                    else {
                        action.off_performFixed();
                    }
                    continue;
                }

                if ( action.flag.ready_to_fixedupdate ) {
                    // call action methods in FixedUpdate()
                    act(action);

                    // ready_to_fixedupdate flag : true -> false
                    action.flag.doneFixedUpdate();
                }
                else {
                    action.off_performFixed();
                }
            }
        }

        public void act(Action action) {
            action.prepare();
            action.performFixed();
            action.effect();
            action.end();
        }

        public void act(string action_name) {
            Action action = actions[action_name];
            act(action);
        }

        public void registerAction(Action action) {
            actions[action.name()] = action;
        }

        public Action getAction(string name) {
            return actions[name];
        }
    }
}
