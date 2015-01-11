using UnityEngine;
using System.Collections;

public class PlayerSliding : PlayerAction {

	public PlayerSliding(Character character) : base(character){
	}

    public override string name() {
        return "SLIDING";
    }

	public override void performFixed() {
        player.rigidbody.AddForce(player.transform.forward * 8000.0f);
	}

    public override void perform() {
		player.getAnimator().CrossFade("Sliding", 0.001f);
        player.lockInput(40);
    }

	public override bool condition(){
		return controller.keySliding() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
    }

}
