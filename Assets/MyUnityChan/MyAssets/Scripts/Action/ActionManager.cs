using UnityEngine;
using System.Collections.Generic;

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

    void FixedUpdate() {
        update();

        foreach ( KeyValuePair<string,Action> pair in actions ) {
            Action action = pair.Value;
            act(action);
        }
    }

    public void act(Action action) {
        action.update();

        if ( action.condition() ) {
            action.prepare();
            action.perform();
            action.effect();
            action.end();
        }
        else {
            action.perform_off();
        }
    }

	public void act(string action_name){
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
