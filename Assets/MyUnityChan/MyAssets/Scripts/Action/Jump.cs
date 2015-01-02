﻿using UnityEngine;
using System.Collections;

public class PlayerJump : PlayerAction {
	protected float jump_start_y;
    protected Vector3 effect_offset = new Vector3(0.0f, 0.2f, 0.0f);

	public PlayerJump(Character character) : base(character){
	}

    public override string name() {
        return "JUMP";
    }

	public override void perform() {
		jump_start_y = player.transform.position.y;
		player.rigidbody.AddForce(new Vector3(0f, 1200.0f,0));
		player.getAnimator().CrossFade("Jump",0.001f);
		player.getAnimator().SetBool("OnGround", false);
	}

	public override bool condition(){
		return controller.keyJump() && player.isGrounded();
	}

    public override bool effect() {
        GameObject effect = UnityEngine.Object.Instantiate(player.jump_effect_prefab) as GameObject;
        ParticleEffect jump_effect = effect.GetComponent<ParticleEffect>();
        jump_effect.init(player.transform.position + effect_offset);
        return true;
    }
}


public class PlayerAirJump : PlayerJump {
    private int jump_num;
    private int jump_max;

    public PlayerAirJump(Character character) : base(character) {
        jump_num = 0;
        jump_max = 2;
    }

    public override string name() {
        return "AIR_JUMP";
    }

    public override void perform() {
        jump_start_y = player.transform.position.y;
        player.getAnimator().SetBool("Jump",true);
        if (player.isDash()) {
            // dashdump (ground jump)
            player.rigidbody.AddForce(new Vector3(player.transform.forward.x * 800.0f, 800.0f, 0));
            player.getAnimator().Play("DashJump", -1, 0.0f);
        }
        else {
            // jump (ground jump or air jump)
            player.rigidbody.AddForce(new Vector3(0f, 1200.0f, 0));
            player.getAnimator().Play("Jump", -1, 0.0f);
        }
        player.setAnimSpeedDefault();
        player.getAnimator().SetBool("OnGround", false);
    }

    public override bool condition() {
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
        if ( scvy < 16.0f ) {
            return true;
        }

        return false;
    }
}
