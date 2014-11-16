using UnityEngine;
using System.Collections;

public class PlayerBrake : PlayerActionBase {
    private const float BRAKE_POWER_DEFAULT = 30.0f;
    private float brake_power;

    public PlayerBrake(Character character) : base(character) {
        brake_power = BRAKE_POWER_DEFAULT;
    }

    public override void perform(Character character) {
        if ( !player.getMoveController().isPlayerInputLocked() ) {
            float horizontal = controller.keyHorizontal();
            Vector3 fw = player.transform.forward;
            float vx = player.rigidbody.velocity.x;
            float vy = player.rigidbody.velocity.y;

            if ( Mathf.Abs(horizontal) < 0.2 && Mathf.Abs(vx) > 0.2f ) {
                // brake down if no input
                if ( Mathf.Sign(fw.x) == Mathf.Sign(vx) ) {
                    player.rigidbody.AddForce(fw * -1 * brake_power);
                }
                else {
                    player.rigidbody.AddForce(fw * brake_power);
                }
            }
        }
    }

    public override bool condition(Character character) {
        return player.isGrounded();
    }
}

public class PlayerAccel : PlayerActionBase {
    private float maxspeed = 20.0f;
    private Vector3 moveF = new Vector3(200f, 0, 0);

    public PlayerAccel(Character character)
        : base(character) {
    }

    public override void perform(Character character) {
        float horizontal = controller.keyHorizontal();
        Vector3 fw = player.transform.forward;
        float vx = player.rigidbody.velocity.x;
        float vy = player.rigidbody.velocity.y;

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
                player.getAnimator().SetBool("Turn", false);
                player.setTurnDirSwitched(false);
            }
        }
        else {
            player.getAnimator().SetBool("Turn", false);
            player.setTurnDirSwitched(false);
        }    

    }

    public override bool condition(Character character) {
        return !player.getMoveController().isPlayerInputLocked();
    }
}

public class PlayerDash : PlayerActionBase {
    private bool dash;
    private Vector3 moveF = new Vector3(200f, 0, 0);

    public PlayerDash(Character character) : base(character){
        dash = false;
    }

    public override void perform(Character character) {
        float horizontal = controller.keyHorizontal();
        dash = true;
        player.getAnimator().speed = player.getAnimSpeedDefault() * 1.4f;
        if ( !player.isTouchedWall() ) {
            player.rigidbody.AddForce(horizontal * moveF);
        }
    }

    public override void perform_off() {
        player.getAnimator().speed = player.getAnimSpeedDefault();
        if ( dash ) {
            dash = false;
            if (player.getAnimator().GetBool("Jump")) {
                return;
            }
            player.getAnimator().CrossFade("Locomotion", 0.01f);
        }
    }

    public override bool condition(Character character) {
        return controller.keyDash() && player.isGrounded();
    }

    public bool isDash() {
        return dash;
    }
}


public class PlayerLimitSpeed : PlayerActionBase {
   

    public PlayerLimitSpeed(Character character)
        : base(character) {
    }

    public override void perform(Character character) {
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

}

