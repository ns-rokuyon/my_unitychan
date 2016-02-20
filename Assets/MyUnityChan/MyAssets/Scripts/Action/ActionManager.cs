﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
            // Orider by each action priority
            var action_orders = actions.OrderByDescending(pair => pair.Value.priority); 

            foreach ( KeyValuePair<string, Action> pair in action_orders ) {
                Action action = pair.Value;

                if ( !action.activation ) {
                    continue;
                }

                // call action methods in Update() constantly
                action.constant_perform();

                if ( action.flag == null ) {
                    if ( action.condition() ) {
                        action.perform();

                        if ( action.skip_lower_priority ) {
                            // If action.skip_lower_priority is true,
                            // ignore remaining actions have lower priority than this action
                            break;
                        }
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

                    if ( action.skip_lower_priority ) {
                        // If action.skip_lower_priority is true,
                        // ignore remaining actions have lower priority than this action
                        break;
                    }
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

        void LateUpdate() {
            foreach ( KeyValuePair<string, Action> pair in actions ) {
                Action action = pair.Value;
                if ( action.condition() ) {
                    action.performLate();
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

        public void disableAction(string name) {
            actions[name].disable();
        }

        public Action getAction(string name) {
            if ( !actions.ContainsKey(name) ) {
                return null;
            }
            return actions[name];
        }

        public List<string> getAllActionKeys() {
            return actions.Select(a => a.Key).ToList();
        }
    }
}
