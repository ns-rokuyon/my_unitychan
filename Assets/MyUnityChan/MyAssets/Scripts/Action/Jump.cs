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


public class PlayerAirJump : PlayerActionBase {
    private float jump_start_y;
    private int jump_num;
    private int jump_max;

    public PlayerAirJump(Character character) : base(character) {
        jump_num = 0;
        jump_max = 100;
    }

    public override void perform(Character character) {
        jump_start_y = player.transform.position.y;
        player.rigidbody.AddForce(new Vector3(0f, 1200.0f, 0));
        player.getAnimator().Play("Locomotion");
        player.getAnimator().SetBool("Jump",true);
        player.getAnimator().Play("Jump");
        player.setAnimSpeedDefault();
        player.getAnimator().SetBool("OnGround", false);

        GameObject effect = UnityEngine.Object.Instantiate(player.jump_effect_prefab) as GameObject;
        ParticleEffect jump_effect = effect.GetComponent<ParticleEffect>();
        jump_effect.init(character.transform.position);
    }

    public override bool condition(Character character) {
        return controller.keyJump() &&
            readyToJump() &&
            jump_num < jump_max ;
    }

    public override bool end() {
        jump_num++;
        return true;
    }

    public override bool update() {
        if ( player.isGrounded() ) {
            jump_num = 0;
        }
        return true;
    }

    private bool readyToJump() {
        if ( player.isGrounded() ) {
            return true;
        }

        float scvy = Mathf.Abs(player.rigidbody.velocity.y);
        if ( scvy < 3.0f ) {
            return true;
        }

        return false;
    }
}
