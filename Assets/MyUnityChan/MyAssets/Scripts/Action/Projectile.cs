using UnityEngine;
using System.Collections;

public class PlayerHadouken : PlayerActionBase {

	public PlayerHadouken(Character character) : base(character){
	}
	
	public override void perform(Character character) {
		Vector3 fw = player.transform.forward;
		player.rigidbody.AddForce(fw * -50.0f);
		player.getAnimator().Play("Hadouken");
		player.getMoveController().register(new Player.MoveLock(30));
		player.getMoveController().register(new Player.DelayDirectionEvent(15, fw, shootProjectile));
	}
	
	public override bool condition(Character character){
		AnimatorStateInfo anim_state = player.getAnimator().GetCurrentAnimatorStateInfo(0);
		
		bool cond = 
			controller.keyProjectile() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded() && 
			anim_state.nameHash != Animator.StringToHash("Base Layer.Hadouken");

		return cond;
	}

	void shootProjectile(Vector3 direction){
		GameObject projectile = UnityEngine.Object.Instantiate(player.projectile_prefab) as GameObject;
		GameObject projectile_particle = UnityEngine.Object.Instantiate(player.projectile_particle_prefab) as GameObject;

		Projectile prjc = projectile.GetComponent<Projectile>();
		Projectile particle = projectile_particle.GetComponent<Projectile>();

		prjc.init(player.transform.position, direction);
		particle.init(player.transform.position, direction, 0.001f);
		if (player.transform.forward.x < 0.0f) {
			particle.transform.Rotate(0.0f, 90.0f, 0.0f);
		}else {
			particle.transform.Rotate(0.0f, -90.0f, 0.0f);
		}
	}
}

