using UnityEngine;
using System.Collections;

public class PlayerBrake : PlayerAction {
    private const float BRAKE_POWER_DEFAULT = 100.0f;
    private float brake_power;

    public PlayerBrake(Character character) : base(character) {
        brake_power = BRAKE_POWER_DEFAULT;
        flag = null;
    }

    public override string name() {
        return "BRAKE";
    }

    public override void performFixed() {
        Vector3 fw = player.transform.forward;
        float vx = player.rigidbody.velocity.x;

        // brake down if no input
        if ( Mathf.Sign(fw.x) == Mathf.Sign(vx) ) {
            player.rigidbody.AddForce(fw * -1 * brake_power);
        }
        else {
            player.rigidbody.AddForce(fw * brake_power);
        }
    }

    public override bool condition() {
        float horizontal = controller.keyHorizontal();
        float vx = player.rigidbody.velocity.x;
        return player.isGrounded() &&
            !player.getMoveController().isPlayerInputLocked() &&
            (Mathf.Abs(horizontal) < 0.2 && Mathf.Abs(vx) > 0.2f);
    }
}

public class PlayerAccel : PlayerAction {
    private float maxspeed = 20.0f;
    private Vector3 moveF = new Vector3(200f, 0, 0);

    public PlayerAccel(Character character)
        : base(character) {
    }

    public override string name() {
        return "ACCEL";
    }

    public override void performFixed() {
        float horizontal = controller.keyHorizontal();
        Vector3 fw = player.transform.forward;
        float vx = player.rigidbody.velocity.x;

        if ( Mathf.Abs(horizontal) >= 0.2 && horizontal * vx < maxspeed ) {
            if ( Mathf.Sign(horizontal) != Mathf.Sign(vx) && Mathf.Abs(vx) > 0.1f ) {
                // when player is turning, add low force
                if (player.isDash()) {
                    player.rigidbody.AddForce(horizontal * moveF / 8.0f);
                }
                else {
                    player.rigidbody.AddForce(horizontal * moveF / 4.0f);
                }
            }
            else {
                // accelerate
                if ( !player.isTouchedWall() ) {
                    player.rigidbody.AddForce(horizontal * moveF);
                }
            }
        }
    }

    public override void perform() {
        float horizontal = controller.keyHorizontal();
        float vx = player.rigidbody.velocity.x;

        if ( Mathf.Abs(horizontal) >= 0.2 && horizontal * vx < maxspeed ) {
            if ( Mathf.Sign(horizontal) != Mathf.Sign(vx) && Mathf.Abs(vx) > 0.1f ) {
            }
            else {
                player.getAnimator().SetBool("Turn", false);
                player.setTurnDirSwitched(false);
            }
        }
        else {
            player.getAnimator().SetBool("Turn", false);
            player.setTurnDirSwitched(false);
        }    
    }

    public override bool condition() {
        return !player.getMoveController().isPlayerInputLocked();
    }
}

public class PlayerDash : PlayerAction {
    private bool dash;
    private Vector3 moveF = new Vector3(200f, 0, 0);

    public PlayerDash(Character character) : base(character){
        dash = false;
    }

    public override string name() {
        return "DASH";
    }

    public override void perform() {
        dash = true;
        player.getAnimator().speed = player.getAnimSpeedDefault() * 1.4f;
    }

    public override void performFixed() {
        float horizontal = controller.keyHorizontal();
        if ( !player.isTouchedWall() ) {
            player.rigidbody.AddForce(horizontal * moveF);
        }
    }

    public override void off_perform() {
        player.getAnimator().speed = player.getAnimSpeedDefault();
        if ( dash ) {
            dash = false;
            if (player.getAnimator().GetBool("Jump")) {
                return;
            }
            player.getAnimator().CrossFade("Locomotion", 0.01f);
        }
    }

    public override bool condition() {
        return controller.keyDash() && player.isGrounded();
    }

    public bool isDash() {
        return dash;
    }
}


public class PlayerLimitSpeed : PlayerAction {

    public PlayerLimitSpeed(Character character)
        : base(character) {
            flag = null;
    }

    public override string name() {
        return "LIMIT_SPEED";
    }

    public override void performFixed() {
        if ( player.isDash() ) {
            limitSpeed(player.dash_maxspeed);
            return;
        }
        if ( player.isAnimState("Base Layer.DashJump") ) {
            limitSpeed(player.dashjump_maxspeed);
            return;
        }

        limitSpeed(player.maxspeed);
    }

    private void limitSpeed(float maxspeed) {
        float vx = player.rigidbody.velocity.x;
        float vy = player.rigidbody.velocity.y;
        if ( Mathf.Abs(vx) > maxspeed ) {
            player.rigidbody.velocity = new Vector3(Mathf.Sign(vx) * maxspeed, vy);
        }
    }

    public override bool condition() {
        return true;
    }

}
