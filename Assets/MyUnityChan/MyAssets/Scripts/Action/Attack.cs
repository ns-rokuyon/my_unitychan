using UnityEngine;
using System.Collections;

public class PlayerAttack : PlayerActionBase {
	private PlayerPunchL left_punch;
	private PlayerPunchR right_punch;
	private PlayerSpinKick spinkick;

	public PlayerAttack(Character character) : base(character){
		left_punch = new PlayerPunchL(character);
		right_punch = new PlayerPunchR(character);
		spinkick = new PlayerSpinKick(character);
	}

	public override void perform(Character character) {
		if (player.isAnimState("Base Layer.PunchR") && spinkick.condition(character)) {
			spinkick.perform(character);
			return;
		}
		else if (player.isAnimState("Base Layer.PunchL") && right_punch.condition(character)) {
			right_punch.perform(character);
			return;
		}
		else {
			left_punch.perform(character);
		}
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded() && 
			!player.isAnimState("Base Layer.SpinKick");
		return cond;
	}
}


public class PlayerPunchL : PlayerActionBase {

	public PlayerPunchL(Character character) : base(character){
	}

	public override void perform(Character character) {
		player.getAnimator().Play("PunchL");
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}
}

public class PlayerPunchR : PlayerActionBase {

	public PlayerPunchR(Character character) : base(character){
	}

	public override void perform(Character character) {
		player.getAnimator().Play("PunchR");
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}
}

public class PlayerSpinKick : PlayerActionBase {

	public PlayerSpinKick(Character character) : base(character){
	}

	public override void perform(Character character) {
		player.getAnimator().Play("SpinKick");
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}
}