using UnityEngine;
using System.Collections;

public class PlayerSliding : PlayerActionBase {

	public PlayerSliding(Character character) : base(character){
	}

	public override void perform(Character character) {
		((Player)character).getAnimator().CrossFade("Sliding", 0.001f);
	}

	public override bool condition(Character character){
		return controller.keySliding() && !((Player)character).getAnimator().GetBool("Turn") && ((Player)character).isGrounded();
    }

}
