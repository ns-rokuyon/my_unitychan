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

public abstract class AttackSpec {
    public int damage { get; set; }
    public int stun { get; set; }       // time enemy is stuned
    public int frame { get; set; }      // time hitbox enabled
    public abstract void attack(Character character, Hitbox hitbox);
}

public class PlayerPunchL : PlayerActionBase {
    public class Spec : AttackSpec {
        public Spec() {
            damage = 10;
            stun = 60;
            frame = 5;
        }

        public override void attack(Character character, Hitbox hitbox) {
            ((Enemy)character).stun(stun);
            character.rigidbody.AddForce(new Vector3(hitbox.forward.x * 5.0f, 5.0f, 0.0f), ForceMode.Impulse);
        }
    }

    public AttackSpec spec = null;

	public PlayerPunchL(Character character) : base(character){
        spec = new Spec();
	}

	public override void perform(Character character) {
		player.getAnimator().Play("PunchL");
		player.getMoveController().register(new Player.DelayNormalEvent(3, createHitbox));
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}

    void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.punch_hitbox_prefab) as GameObject;
        PunchHitbox hitbox_script = hitbox.GetComponent<PunchHitbox>();
        hitbox_script.create(player.transform.position, player.transform.forward, spec);
    }
}

public class PlayerPunchR : PlayerActionBase {
    public class Spec : AttackSpec {
        public Spec() {
            damage = 20;
            stun = 120;
            frame = 5;
        }

        public override void attack(Character character, Hitbox hitbox) {
            ((Enemy)character).stun(stun);
            character.rigidbody.AddForce(new Vector3(hitbox.forward.x * 2.0f, 7.0f, 0.0f), ForceMode.Impulse);
        }
    }

    public AttackSpec spec = null;

	public PlayerPunchR(Character character) : base(character){
        spec = new Spec();
	}

	public override void perform(Character character) {
		player.getAnimator().Play("PunchR");
		player.getMoveController().register(new Player.DelayNormalEvent(6, createHitbox));
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}

    private void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.punch_hitbox_prefab) as GameObject;
        PunchHitbox hitbox_script = hitbox.GetComponent<PunchHitbox>();
        hitbox_script.create(player.transform.position + player.transform.forward * 1.3f, player.transform.forward, spec);
    }
}

public class PlayerSpinKick : PlayerActionBase {
    public class Spec : AttackSpec {
        public Spec() {
            damage = 20;
            stun = 120;
            frame = 12;
        }

        public override void attack(Character character, Hitbox hitbox) {
            ((Enemy)character).stun(stun);
            character.rigidbody.AddForce(new Vector3(hitbox.forward.x * 2.0f, 7.0f, 0.0f), ForceMode.Impulse);
        }
    }

    public AttackSpec spec = null;

	public PlayerSpinKick(Character character) : base(character){
        spec = new Spec();
	}

	public override void perform(Character character) {
		player.getAnimator().Play("SpinKick");
		player.getMoveController().register(new Player.DelayNormalEvent(20, createHitbox));
	}

	public override bool condition(Character character){
		bool cond = 
			controller.keyAttack() && 
			!player.getAnimator().GetBool("Turn") && 
			player.isGrounded();
		return cond;
	}

    private void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.kick_hitbox_prefab) as GameObject;
        KickHitbox hitbox_script = hitbox.GetComponent<KickHitbox>();
        hitbox_script.create(player.transform.position + player.transform.forward * 1.3f, player.transform.forward, spec);
    }
}