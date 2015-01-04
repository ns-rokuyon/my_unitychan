using UnityEngine;
using System.Collections;

public class PlayerAttack : PlayerAction {
	private PlayerPunchL left_punch;
	private PlayerPunchR right_punch;
	private PlayerSpinKick spinkick;

    public override string name() {
        return "ATTACK";
    }

	public PlayerAttack(Character character) : base(character){
		left_punch = new PlayerPunchL(character);
		right_punch = new PlayerPunchR(character);
		spinkick = new PlayerSpinKick(character);
	}

	public override void performFixed() {
		if (player.isAnimState("Base Layer.PunchR")) {
			spinkick.performFixed();
		}
		else if (player.isAnimState("Base Layer.PunchL")) {
			right_punch.performFixed();
		}
		else {
			left_punch.performFixed();
		}
	}

	public override void perform() {
		if (player.isAnimState("Base Layer.PunchR")) {
			spinkick.perform();
		}
		else if (player.isAnimState("Base Layer.PunchL")) {
			right_punch.perform();
		}
		else {
			left_punch.perform();
		}
	}

	public override bool condition(){
        bool cond = false;
        if ( player.isAnimState("Base Layer.PunchR") ) {
            cond = spinkick.condition();
		}
        else if ( player.isAnimState("Base Layer.PunchL") ) {
			cond = right_punch.condition();
		}
        else if ( player.isAnimState("Base Layer.SpinKick") ) {
            cond = false;
        }
        else {
            cond = left_punch.condition();
        }
        return cond;
	}

}

public abstract class AttackSpec {
    public int damage { get; set; }
    public int stun { get; set; }       // time enemy is stuned
    public int frame { get; set; }      // time hitbox enabled
    public abstract void attack(Character character, Hitbox hitbox);
}

public class PlayerPunchL : PlayerAction {
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

    public override string name() {
        return "PUNCH_L";
    }

    public override void perform() {
		player.getAnimator().Play("PunchL");
        InvokerManager.createFrameDelayInvoker(3, createHitbox);
    }

	public override bool condition(){
        bool cond =
            controller.keyAttack() &&
            !player.getAnimator().GetBool("Turn");
		return cond;
	}

    void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.punch_hitbox_prefab) as GameObject;
        PunchHitbox hitbox_script = hitbox.GetComponent<PunchHitbox>();
        hitbox_script.create(player.transform.position, player.transform.forward, spec);
    }
}

public class PlayerPunchR : PlayerAction {
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

    public override string name() {
        return "PUNCH_R";
    }

    public override void perform() {
		player.getAnimator().Play("PunchR");
        InvokerManager.createFrameDelayInvoker(6, createHitbox);
		//player.getMoveController().register(new Player.DelayNormalEvent(6, createHitbox));
    }

	public override bool condition(){
        bool cond =
            controller.keyAttack() &&
            !player.getAnimator().GetBool("Turn");
		return cond;
	}

    private void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.punch_hitbox_prefab) as GameObject;
        PunchHitbox hitbox_script = hitbox.GetComponent<PunchHitbox>();
        hitbox_script.create(player.transform.position + player.transform.forward * 1.3f, player.transform.forward, spec);
    }
}

public class PlayerSpinKick : PlayerAction {
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

    public override string name() {
        return "SPIN_KICK";
    }

    public override void perform() {
		player.getAnimator().Play("SpinKick");
        InvokerManager.createFrameDelayInvoker(20, createHitbox);
    }

	public override bool condition(){
        bool cond =
            controller.keyAttack() &&
            !player.getAnimator().GetBool("Turn");
		return cond;
	}

    private void createHitbox() {
        GameObject hitbox = GameObject.Instantiate(player.kick_hitbox_prefab) as GameObject;
        KickHitbox hitbox_script = hitbox.GetComponent<KickHitbox>();
        hitbox_script.create(player.transform.position + player.transform.forward * 1.3f, player.transform.forward, spec);
    }
}