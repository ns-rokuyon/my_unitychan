using UnityEngine;
using System.Collections.Generic;

public class ActionManager {
	protected Character character;

	public ActionManager(){
		character = null;
	}

	public ActionManager(Character c){
		character = c;
	}
}

public class EnemyActionManager : ActionManager {
    protected Dictionary<string, ActionBase> actions;

    public EnemyActionManager() {
        actions = new Dictionary<string, ActionBase>();
    }

    public void registerAction(string key, ActionBase value) {
        actions[key] = value;
    }
    
	public void act(string action_name){
		ActionBase action = actions[action_name];

        action.update();

        if ( action.condition(character) ) {
            action.prepare();
            action.perform(character);
            action.effect();
            action.end();
        }
        else {
            action.perform_off();
        }
	}
}

public class PlayerActionManager : ActionManager{
	public enum ActionName {
        BRAKE,
        ACCEL,
        DASH,
        LIMIT_SPEED,
        AIR_JUMP,
		SLIDING,
		PROJECTILE,
		ATTACK,
		TURN
	}
	private List<ActionBase> action_list;

	public PlayerActionManager(Character ch) : base(ch){
		action_list = new List<ActionBase>();
        action_list.Add(new PlayerBrake(ch));
        action_list.Add(new PlayerAccel(ch));
        action_list.Add(new PlayerDash(ch));
        action_list.Add(new PlayerLimitSpeed(ch));
        action_list.Add(new PlayerAirJump(ch));
		action_list.Add(new PlayerSliding(ch));
		action_list.Add(new PlayerHadouken(ch));
		action_list.Add(new PlayerAttack(ch));
		action_list.Add(new PlayerTurn(ch));
	}

	public void act(ActionName action_name){
		ActionBase action = action_list[(int)action_name];

        action.update();

        if ( action.condition(character) ) {
            action.prepare();
            action.perform(character);
            action.effect();
            action.end();
        }
        else {
            action.perform_off();
        }
	}

    public ActionBase getAction(ActionName action_name) {
        return action_list[(int)action_name];
    }

}

public abstract class ActionBase {
	public abstract void perform(Character character);

    public virtual void perform_off() {
    }

	public virtual bool condition(Character character){
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

public abstract class PlayerActionBase : ActionBase {
	protected Player player;
	protected PlayerController controller;

	public PlayerActionBase(Character character) {
		player = (Player)character;
		controller = (PlayerController)player.getController();
	}

}

public abstract class EnemyActionBase : ActionBase {
	protected Enemy enemy;
	protected AIController controller;

	public EnemyActionBase(Character character) {
		enemy = (Enemy)character;
		controller = (AIController)enemy.getController();
	}

}
