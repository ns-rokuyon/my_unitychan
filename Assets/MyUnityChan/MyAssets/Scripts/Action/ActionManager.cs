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

public class PlayerActionManager : ActionManager{
	public enum ActionName {
		JUMP,
		SLIDING,
		PROJECTILE
	}
	private List<ActionBase> action_list;

	public PlayerActionManager(Character ch) : base(ch){
		action_list = new List<ActionBase>();
		action_list.Add(new PlayerJump(ch));
		action_list.Add(new PlayerSliding(ch));
		action_list.Add(new PlayerHadouken(ch));
	}

	public void act(ActionName action_name){
		ActionBase action = action_list[(int)action_name];
		if (action.condition(character)) {
			action.perform(character);
		}
	}

}

public abstract class ActionBase {
	public abstract void perform(Character character);
	public virtual bool condition(Character character){
		return true;
	}
}

public abstract class PlayerActionBase : ActionBase {
	protected Player player;
	protected Controller controller;

	public PlayerActionBase(Character character) {
		player = (Player)character;
		controller = player.getController();
	}

}