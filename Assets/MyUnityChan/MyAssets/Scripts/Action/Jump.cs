using UnityEngine;
using System.Collections;

public class PlayerJump : PlayerActionBase {
	private float jump_start_y;

	public PlayerJump(Character character) : base(character){
	}

	public override void perform(Character character) {
		Player player = (Player)character;
		jump_start_y = player.transform.position.y;
		player.rigidbody.AddForce(new Vector3(0f, 1200.0f,0));
		player.getAnimator().CrossFade("Jump",0.001f);
		player.getAnimator().SetBool("OnGround", false);
		
		GameObject effect = UnityEngine.Object.Instantiate(player.jump_effect_prefab) as GameObject;
		ParticleEffect jump_effect = effect.GetComponent<ParticleEffect>();
		jump_effect.init(character.transform.position);
	}

	public override bool condition(Character character){
		return controller.keyJump() && ((Player)character).isGrounded();
	}
}
