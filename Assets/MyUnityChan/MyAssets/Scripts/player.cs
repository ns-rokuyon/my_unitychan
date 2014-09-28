using UnityEngine;
using System.Collections;

using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]  

//Name of class must be name of file as well

public class player : Character {

	public GameObject projectile_prefab;
	public GameObject controller_prefab;

	Animator animator;
	private MoveControlManager move_controller = null;

	public Vector3 moveF = new Vector3(200f, 0, 0);
	public float maxspeed = 20f;

	private float speed = 0;
	private float direction = 0;
	private Locomotion locomotion = null;
	private Vector3 moveDirection= Vector3.zero;
	private Vector3 old_forward;
	private Vector3 turn_slide_formard;
	private int anim_turn_id;
	private bool on_ground = true;
	private float dist_to_ground;
	private Vector3 dist_checksphere_center = new Vector3(0,0.2f,0);
	private float anim_speed_default;
	private float jump_start_y ;		// jump start point y
	private bool turn_dir_switched = false;

	private const float CHECKSPHERE_RADIUS = 0.1f;	// radius of sphere to check player is on ground


	// Use this for initialization
	void Start () 
	{
		GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
		controller = controller_inst.GetComponent<Controller>();

		old_forward = transform.forward;
		animator = GetComponent<Animator>();
		locomotion = new Locomotion(animator);
		anim_speed_default = animator.speed * 1.2f;
		dist_to_ground = GetComponent<CapsuleCollider>().height;
		move_controller = new MoveControlManager();
	}

	void Update()
	{
	}


	void FixedUpdate(){
		move_controller.update();
		float horizontal = controller.keyHorizontal();

		AnimatorStateInfo anim_state = animator.GetCurrentAnimatorStateInfo(0);

		animator.SetFloat ("Speed", Mathf.Abs (horizontal));

		float vx = rigidbody.velocity.x;
		float vy = rigidbody.velocity.y;
		Vector3 fw = transform.forward;

		if (vy < 0 && isGrounded()){
			// landing
			animator.SetBool("OnGround",true);
			animator.speed = anim_speed_default;

		}

		if (!move_controller.isPlayerInputLocked()){
			if (Mathf.Abs (horizontal) >= 0.2 && horizontal * vx < maxspeed) {
				if (Mathf.Sign (horizontal) != Mathf.Sign (vx) && Mathf.Abs(vx) > 0.1f) {
					// when player is turning, add low force
					rigidbody.AddForce (horizontal * moveF / 4.0f);
					if (isGrounded() && turn_dir_switched == false){
						animator.CrossFade("PlantNTurnRight180",0.01f);
						animator.SetBool ("Turn", true);
					}
				} else {
					// accelerate
					rigidbody.AddForce (horizontal * moveF);
					animator.SetBool ("Turn", false);
					turn_dir_switched = false;
				}
			} else {
				animator.SetBool("Turn", false);
				turn_dir_switched = false;
			}
	
	
			if (Mathf.Abs(horizontal) < 0.2 && Mathf.Abs(vx) > 0.2f) {
				// brake down if no input
				if (Mathf.Sign(fw.x) == Mathf.Sign(vx)) {
					rigidbody.AddForce(fw * -20.0f);
				} else {
					rigidbody.AddForce(fw * 20.0f);
				}
			}
		}

		if (Mathf.Abs (vx) > maxspeed) {
			rigidbody.velocity = new Vector3(Mathf.Sign(vx)* maxspeed, vy);
		}

		
		if (horizontal > 0 && fw.x < 0) {
			// input right when player turns left
			transform.rotation = Quaternion.LookRotation (new Vector3 (horizontal, 0, 0.8f));
		} else if (horizontal < 0 && fw.x > 0) {
			// input left when player turns right
			transform.rotation = Quaternion.LookRotation (new Vector3 (horizontal, 0, -0.8f));
		} else {
			float newz_fw = fw.z;
			if ( newz_fw > 0 ) {
				newz_fw -= 0.3f;
				if (newz_fw < 0) {
					newz_fw = 0;
				}
			}
			else {
				newz_fw += 0.3f;
				if (newz_fw > 0) {
					newz_fw = 0;
				}
			}
			transform.rotation = Quaternion.LookRotation (new Vector3 (fw.x, 0, newz_fw));
		}

		if (controller.keyJump() && isGrounded()) {
			// jump
			jump_start_y = transform.position.y;
			rigidbody.AddForce(new Vector3(0f, 1200.0f,0));
			animator.CrossFade("Jump",0.001f);
			animator.SetBool("OnGround", false);
		}

		if (controller.keySliding() && !animator.GetBool("Turn") && isGrounded()) {
			// sliding
			animator.CrossFade("Sliding", 0.001f);
		}

		if (controller.keyAttack() && !animator.GetBool("Turn") && isGrounded() && anim_state.nameHash != Animator.StringToHash("Base Layer.SpinKick")) {
			// punch
			Debug.Log ("x:" + controller.keyAttack().ToString());
			if (anim_state.nameHash == Animator.StringToHash("Base Layer.PunchL")) {
				animator.Play("PunchR");
			}
			else if (anim_state.nameHash == Animator.StringToHash("Base Layer.PunchR")) {
				animator.Play("SpinKick");
			}
			else {
				animator.Play("PunchL");
			}		
		}

		if (controller.keyProjectile() && !animator.GetBool("Turn") && isGrounded() && anim_state.nameHash != Animator.StringToHash("Base Layer.Hadouken") ) {
			// hadouken
			rigidbody.AddForce(fw * -50.0f);
			animator.Play("Hadouken");
			move_controller.register(new MoveLock(30));
			move_controller.register(new DelayDirectionEvent(15, fw, shootProjectile));
		}
		
		// gravity
		rigidbody.AddForce(new Vector3(0f, -32.0f,0));	// -26
	}

	void shootProjectile(Vector3 direction){
		GameObject projectile = Instantiate(projectile_prefab) as GameObject;
		Projectile prjc = projectile.GetComponent<Projectile>();
		prjc.init(transform.position, direction);
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
		animator.speed = 0.2f;	// slow animation
	}

	bool isGrounded(){
		// check player is on ground with sphere under the foot
		return Physics.CheckSphere(transform.position - dist_checksphere_center,  CHECKSPHERE_RADIUS);
	}

	void lockInput(){

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
		GUI.Label(new Rect(Screen.width -245,130,250,30),"dist_to_ground: " + dist_to_ground);
		GUI.Label(new Rect(Screen.width -245,150,250,30),"(x,y,z): " + transform.position);
		GUI.Label(new Rect(Screen.width -245,170,250,30),"capsule_center: " + cc.bounds.center);
		GUI.Label(new Rect(Screen.width -245,190,250,30),"capsule_height: " + cc.height);
		GUI.Label(new Rect(Screen.width -245,210,250,30),"x_key_down: " + Input.GetKeyDown("x").ToString());
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