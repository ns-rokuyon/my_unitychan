using UnityEngine;
using System.Collections;

using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]  

//Name of class must be name of file as well

public class Player : Character {

	public GameObject projectile_prefab;
	public GameObject projectile_particle_prefab;
	public GameObject jump_effect_prefab;
	public GameObject controller_prefab;

	private Animator animator;
	private MoveControlManager move_controller = null;
	private PlayerActionManager action_manager = null;

	public Vector3 moveF = new Vector3(200f, 0, 0);
	public float maxspeed = 20f;
    public float dash_maxspeed = 30.0f;

    private float brake_power = 30.0f;
	private float speed = 0;
	private Locomotion locomotion = null;
	private Vector3 turn_slide_formard;
	private int anim_turn_id;
	private float dist_to_ground;
	private Vector3 dist_checksphere_center = new Vector3(0,0.6f,0);
    private Vector3 ground_raycast_offset = new Vector3(0, 0.05f, 0);
	private float anim_speed_default;
	private float jump_start_y ;		// jump start point y
	private bool turn_dir_switched = false;

	private const float CHECKSPHERE_RADIUS = 0.1f;	// radius of sphere to check player is on ground
    private GameObject sphere_ground_check = null;

	// Use this for initialization
	void Start () 
	{
		GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
		controller = controller_inst.GetComponent<Controller>();

		animator = GetComponent<Animator>();
		locomotion = new Locomotion(animator);
		anim_speed_default = animator.speed * 1.2f;
		dist_to_ground = GetComponent<CapsuleCollider>().height;
		move_controller = new MoveControlManager();
		action_manager = new PlayerActionManager(this);

	}

	void Update()
	{
		move_controller.update();
	}


	void FixedUpdate(){
		float horizontal = controller.keyHorizontal();

		animator.SetFloat ("Speed", Mathf.Abs (horizontal));

		float vy = rigidbody.velocity.y;

		if (vy <= 0 && isGrounded()){
			// landing
			animator.SetBool("OnGround",true);
			animator.speed = anim_speed_default;
            animator.SetBool("Jump", false);
		}

        // accelerate player in response to input
        action_manager.act(PlayerActionManager.ActionName.ACCEL);

        // dash
        action_manager.act(PlayerActionManager.ActionName.DASH);

		// limit speed (maxspeed)
        action_manager.act(PlayerActionManager.ActionName.LIMIT_SPEED);

        // brake player unless there is input
        action_manager.act(PlayerActionManager.ActionName.BRAKE);

		// turn
		action_manager.act(PlayerActionManager.ActionName.TURN);

		// jump on ground or second jump in air
		action_manager.act(PlayerActionManager.ActionName.AIR_JUMP);

		// sliding
		action_manager.act(PlayerActionManager.ActionName.SLIDING);

		// atttack (punchL -> punchR -> spinkick)
		action_manager.act(PlayerActionManager.ActionName.ATTACK);

		// hadouken (shoot a projectile)
		action_manager.act(PlayerActionManager.ActionName.PROJECTILE);

		// gravity
		rigidbody.AddForce(new Vector3(0f, -32.0f,0));	// -26
	}


	void onTurnMiddle(){
		Debug.Log("on turn middle");
		turn_dir_switched = true;
	}

	void onTurnEnd(){
		turn_dir_switched = false;
		animator.SetBool("Turn", false);
	}

	void OnJumpAnimEnd(){
		// end of jump motion
		Debug.Log("on jump anim end");
        animator.SetBool("Jump", false);
		//animator.speed = 0.0f;	// slow animation
	}

	public bool isGrounded(){
		// check player is on ground with sphere under the foot

		//return Physics.CheckSphere(transform.position - dist_checksphere_center,  CHECKSPHERE_RADIUS);

        return Physics.Raycast(transform.position + ground_raycast_offset, Vector3.down, 0.5f) ||
            Physics.Raycast(transform.position + ground_raycast_offset, new Vector3(1.0f, 0.0f, 0), 1.0f) ||
            Physics.Raycast(transform.position + ground_raycast_offset, new Vector3(-1.0f, 0.0f, 0), 1.0f);
	}

    public bool isTouchedWall() {
        // check player is in front of wall
        CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
        return Physics.Raycast(transform.position + new Vector3(0, capsule_collider.bounds.size.y, 0), transform.forward, 2.0f) ||
            Physics.Raycast(capsule_collider.bounds.center, transform.forward, 2.0f);
    }

	public bool isTurnDirSwitched(){
		return turn_dir_switched;
	}

	void lockInput(){

	}

	public bool isAnimState(string anim_name) {
		AnimatorStateInfo anim_state = animator.GetCurrentAnimatorStateInfo(0);
		return anim_state.nameHash == Animator.StringToHash(anim_name);
	}

	public Animator getAnimator(){
		return animator;
	}

	public MoveControlManager getMoveController(){
		return move_controller;
	}

    public void setAnimSpeedDefault() {
        animator.speed = anim_speed_default;
    }

    public void setTurnDirSwitched(bool flag) {
        turn_dir_switched = flag;
    }

    public float getAnimSpeedDefault() {
        return anim_speed_default;
    }


	void OnGUI()
	{
		Vector3 fw = transform.forward;
		Quaternion rot = transform.rotation;
		Vector3 targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		float vx = rigidbody.velocity.x; 
		float vy = rigidbody.velocity.y;
		CapsuleCollider cc = GetComponent<CapsuleCollider>();
		GUI.Box(new Rect(Screen.width -260, 10 ,250 ,250), "Interaction");
		GUI.Label(new Rect(Screen.width -245,30,250,30),"forward: " + fw);
		GUI.Label(new Rect(Screen.width -245,50,250,30),"vx: " + vx);
		GUI.Label(new Rect(Screen.width -245,70,250,30),"vy: " + vy);
		GUI.Label(new Rect(Screen.width -245,90,250,30),"targetDirection: " + targetDirection);
		GUI.Label(new Rect(Screen.width -245,110,250,30),"on_ground: " + isGrounded());
		GUI.Label(new Rect(Screen.width -245,130,250,30),"touched_wall: " + isTouchedWall());
		GUI.Label(new Rect(Screen.width -245,150,250,30),"(x,y,z): " + transform.position);
		GUI.Label(new Rect(Screen.width -245,170,250,30),"capsule_center: " + cc.bounds.center);
		GUI.Label(new Rect(Screen.width -245,190,250,30),"capsule_height: " + cc.height);
		GUI.Label(new Rect(Screen.width -245,210,250,30),"x_key_down: " + Input.GetKeyDown("x").ToString());
        GUI.Label(new Rect(Screen.width -245,230,250,30),"animspeed: " + animator.speed);
	}
}

/*
 Unitychan Animation

unitychan_JUMP00
	start: 10 - end: 56
	Root Transform Position (Y) : off
	Events:
		* 0.8 : OnJumpAnimEnd()


DefaultAvatar_PlantNTurn180
	start:151 - end: 194.7
	Root Transform Rotation : off
	Root Transform Position (Y) : on
	Root Transform Position (XZ) : off
*/ 