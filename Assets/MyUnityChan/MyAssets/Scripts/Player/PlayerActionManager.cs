using UnityEngine;
using System.Collections.Generic;

public class PlayerActionManager : ActionManager{

    protected override void start() {
    }

    protected override void update() {
    }

    void OnGUI() {
        Action action = getAction("AIR_JUMP");
        GUIStyle gui_style = new GUIStyle();
        GUIStyleState gui_stylestate = new GUIStyleState();
        gui_stylestate.textColor = Color.green;
        gui_style.normal = gui_stylestate;
        string text = "cond: " + action.flag.condition + ", upd: " + action.flag.ready_to_update + ", fixed: " + action.flag.ready_to_fixedupdate;
        GUI.Label(new Rect(10, 70, 250, 30), text);
    }

}

public abstract class PlayerAction : Action {
	protected Player player;
	protected PlayerController controller;

	public PlayerAction(Character character) {
		player = (Player)character;
		controller = (PlayerController)player.getController();
	}

}
