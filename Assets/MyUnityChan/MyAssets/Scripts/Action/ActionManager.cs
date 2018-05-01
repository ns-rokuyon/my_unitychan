using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public abstract class ActionManager : ObjectBase {
        protected Character character;
        protected Dictionary<string, Action> actions = new Dictionary<string, Action>();
        protected Dictionary<System.Type, Action> _type_indexed_actions = new Dictionary<System.Type, Action>();

        protected List<Action> adhoc_perform_actions = new List<Action>();
        protected List<Action> adhoc_performFixed_actions = new List<Action>();
        protected List<Action> adhoc_performLate_actions = new List<Action>();

        protected abstract void start();
        protected abstract void update();

        [SerializeField, ReadOnly]
        private List<string> registered_action_names = new List<string>();

        void Awake() {
            character = GetComponent<Character>();
        }

        void Start() {
            start();
        }

        void Update() {
            if ( character.time_control.paused )
                return;

            // Orider by each action priority
            var action_orders = actions.OrderByDescending(pair => pair.Value.priority); 

            foreach ( KeyValuePair<string, Action> pair in action_orders ) {
                Action action = pair.Value;

                if ( !action.activation ) {
                    continue;
                }

                if ( !action.initialized ) {
                    action.init();
                    action.initialized = true;
                }

                int adhoc = adhoc_perform_actions.IndexOf(action);
                if ( adhoc >= 0 ) {
                    // Force action
                    action.perform();
                    if ( action.perform_callbacks.Count > 0 ) {
                        action.perform_callbacks.ForEach(callback => callback());
                    }
                    adhoc_perform_actions.RemoveAt(adhoc);
                    continue;
                }

                // call action methods in Update() constantly
                action.constant_perform();

                if ( action.flag == null ) {
                    if ( action.condition() ) {
                        action.perform();

                        if ( action.end_perform_callbacks.Count == 0 ) {
                            // Add default end_perform function at first frame of action.condition==true
                            action.end_perform_callbacks.Add(action.end_perform);
                        }

                        if ( action.perform_callbacks.Count > 0 ) {
                            action.perform_callbacks.ForEach(callback => callback());
                        }

                        if ( action.skip_lower_priority ) {
                            // If action.skip_lower_priority is true,
                            // ignore remaining actions have lower priority than this action
                            break;
                        }
                    }
                    else {
                        if ( action.transaction == null ) {
                            if ( action.end_perform_callbacks.Count > 0 ) {
                                // Call end_perform_callbacks at the first off perform frame
                                action.end_perform_callbacks.ForEach(callback => callback());
                                action.end_perform_callbacks.Clear();
                            }

                            action.off_perform();
                        }
                        if ( action.skip_lower_priority && 
                             action.keep_skipping_lower_priority_in_transaction &&
                             action.transaction != null ) {
                            break;
                        }
                    }
                    continue;
                }

                if ( action.flag.condition == false && action.condition() ) {
                    // action start point
                    action.flag.setAll();

                    // call action methods in Update()
                    action.perform();

                    if ( action.end_perform_callbacks.Count == 0 ) {
                        // Add default end_perform function at first frame of action.condition==true
                        action.end_perform_callbacks.Add(action.end_perform);
                    }

                    if ( action.perform_callbacks.Count > 0 ) {
                        action.perform_callbacks.ForEach(callback => callback());
                    }

                    // ready_to_update flag : true -> false
                    action.flag.doneUpdate();

                    if ( action.skip_lower_priority ) {
                        // If action.skip_lower_priority is true,
                        // ignore remaining actions have lower priority than this action
                        break;
                    }
                }
                else {
                    if ( action.transaction == null ) {
                        if ( action.end_perform_callbacks.Count > 0 ) {
                            // Call end_perform_callbacks at the first off perform frame
                            action.end_perform_callbacks.ForEach(callback => callback());
                            action.end_perform_callbacks.Clear();
                        }

                        action.off_perform();
                    }
                    if ( action.skip_lower_priority && 
                         action.keep_skipping_lower_priority_in_transaction &&
                         action.transaction != null ) {
                        break;
                    }
                }
            }
        }

        void FixedUpdate() {
            update();

            foreach ( KeyValuePair<string, Action> pair in actions ) {
                Action action = pair.Value;

                // call action methods in Update() constantly
                action.constant_performFixed();

                int adhoc = adhoc_performFixed_actions.IndexOf(action);
                if ( adhoc >= 0 ) {
                    // Force action
                    act(action);
                    adhoc_performFixed_actions.RemoveAt(adhoc);       // Remove adhoc action from list
                    continue;
                }

                if ( action.flag == null ) {
                    if ( action.condition() ) {
                        // call action methods in FixedUpdate()
                        act(action);
                    }
                    else {
                        if ( action.transaction == null ) {
                            action.off_performFixed();
                        }
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
                    if ( action.transaction == null ) {
                        action.off_performFixed();
                    }
                }
            }
        }

        void LateUpdate() {
            foreach ( KeyValuePair<string, Action> pair in actions ) {
                Action action = pair.Value;

                action.constant_performLate();

                int adhoc = adhoc_performLate_actions.IndexOf(action);
                if ( adhoc >= 0 ) {
                    // Force action
                    action.performLate();
                    adhoc_performLate_actions.RemoveAt(adhoc);       // Remove adhoc action from list
                    continue;
                }

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

        public void forceAction(string action_name) {
            adhoc_perform_actions.Add(getAction(action_name));
            adhoc_performFixed_actions.Add(getAction(action_name));
            adhoc_performLate_actions.Add(getAction(action_name));
        }

        public void registerAction(Action action) {
            if ( actions.ContainsKey(action.name()) ) {
                DebugManager.warn(action.name() + " is already registered");
                return;
            }
            actions[action.name()] = action;
            registered_action_names.Add(action.name());
        }

        public void disableAction(string name) {
            actions[name].disable();
        }

        public void enableAction(string name) {
            actions[name].enable();
        }

        public void disableAllActions() {
            actions.Values.ToList().ForEach(action => action.disable());
        }

        public void enableAllActions() {
            actions.Values.ToList().ForEach(action => action.enable());
        }

        public bool hasAction(string name) {
            return actions.ContainsKey(name);
        }

        public Action getAction(string name) {
            if ( !actions.ContainsKey(name) ) {
                return null;
            }
            return actions[name];
        }

        public T getAction<T>() where T : class {
            System.Type t = typeof(T);
            Action action;

            if ( _type_indexed_actions.ContainsKey(t) ) {
                action = _type_indexed_actions[t];
            }
            else {
                action = actions.Values.FirstOrDefault(a => a.GetType() == t);
                if ( action != null )
                    _type_indexed_actions.Add(t, action);
            }

            if ( action == null )
                return null;
            return action as T;
        }

        public T getAction<T>(string name) where T : class {
            if ( !actions.ContainsKey(name) )
                return null;
            return actions[name] as T;
        }

        public List<string> getAllActionKeys() {
            return actions.Select(a => a.Key).ToList();
        }
    }
}
