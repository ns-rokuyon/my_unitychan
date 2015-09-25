using UnityEngine;
using System.Collections;

using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    [RequireComponent(typeof(Animator))]

    public class Player : Character {

        public string player_name = null;

        public GameObject projectile_prefab;
        public GameObject projectile_particle_prefab;
        public GameObject jump_effect_prefab;
        public GameObject controller_prefab;
        public GameObject action_manager_prefab;

        public GameObject punch_hitbox_prefab;
        public GameObject kick_hitbox_prefab;
        public GameObject projectile_hitbox_prefab;

        private GameObject player_root;
        private PlayerCamera camera;
        private Animator animator;
        private PlayerActionManager action_manager = null;
        private HpGauge hpgauge;

        public Vector3 moveF = new Vector3(200f, 0, 0);
        public float maxspeed = 20f;
        public float dash_maxspeed = 30.0f;
        public float dashjump_maxspeed = 50.0f;

        private float brake_power = 30.0f;
        private float speed = 0;
        //private Locomotion locomotion = null;
        private Vector3 turn_slide_formard;
        private int anim_turn_id;
        private float dist_to_ground;
        private Vector3 dist_checksphere_center = new Vector3(0, 0.6f, 0);
        private Vector3 ground_raycast_offset = new Vector3(0, 0.05f, 0);
        private float anim_speed_default;
        private float jump_start_y;		// jump start point y
        private bool turn_dir_switched = false;

        private const float CHECKSPHERE_RADIUS = 0.1f;	// radius of sphere to check player is on ground
        private GameObject sphere_ground_check = null;

        private string area_name = null;

        protected static GameObject player;

        // Use this for initialization
        void Start() {
            player_name = "player1";
            player_root = transform.parent.gameObject;

            // camera setup
            camera = PrefabInstantiater.createAndGetComponent<PlayerCamera>(Const.Prefab.Camera["PLAYER_CAMERA"], Hierarchy.Layout.CAMERA);
            camera.setPlayer(this.gameObject);

            // controller setup
            controller = (Instantiate(controller_prefab) as GameObject).setParent(player_root).GetComponent<Controller>();
            controller.setSelf(this);

            // action manager setup
            action_manager = (Instantiate(action_manager_prefab) as GameObject).setParent(player_root).GetComponent<PlayerActionManager>();

            // player status setup
            status = (Instantiate(status_prefab) as GameObject).setParent(player_root).GetComponent<PlayerStatus>();

            // HP gauge setup
            hpgauge = PrefabInstantiater.create(Const.Prefab.UI["PLAYER_HP_GAUGE"], HpGauge.getCanvas("Canvas")).GetComponent<HpGauge>();
            hpgauge.setCharacter(this);
            hpgauge.setPosition(new Vector3(200, -24, 10));
            //hpgauge.transform.SetParent(HpGauge.getCanvas().transform, false);

            // animation
            animator = GetComponent<Animator>();
            //locomotion = new Locomotion(animator);
            anim_speed_default = animator.speed * 1.2f;
            dist_to_ground = GetComponent<CapsuleCollider>().height;

            // init timer
            inputlock_timer = new FrameTimerState();

            // init player actions
            registerActions();

            // player infomation for NPC
            NPCharacter.setPlayers();

            // set player to GameStateManager
            GameStateManager.self().player = this;

            player = gameObject;
        }

        void Update() {
            float horizontal = ((PlayerController)controller).keyHorizontal();
            float vy = GetComponent<Rigidbody>().velocity.y;

            animator.SetFloat("Speed", Mathf.Abs(horizontal));
            if ( vy <= 0 && isGrounded() ) {
                // landing
                animator.SetBool("OnGround", true);
                animator.speed = anim_speed_default;
                animator.SetBool("Jump", false);
            }

        }


        void FixedUpdate() {
            // gravity
            //rigidbody.AddForce(new Vector3(0f, -32.0f, 0));	// -32
        }

        public static GameObject getPlayer() {
            return player;
        }

        private void registerActions() {
            action_manager.registerAction(new PlayerBrake(this));
            action_manager.registerAction(new PlayerAccel(this));
            action_manager.registerAction(new PlayerDash(this));
            action_manager.registerAction(new PlayerLimitSpeed(this));
            action_manager.registerAction(new PlayerJump(this));
            action_manager.registerAction(new PlayerSliding(this));
            action_manager.registerAction(new PlayerHadouken(this));
            action_manager.registerAction(new PlayerAttack(this));
            action_manager.registerAction(new PlayerTurn(this));
            action_manager.registerAction(new PlayerDown(this));
            action_manager.registerAction(new PlayerBeam(this));
        }

        public override void damage(int dam) {
            if ( !status.invincible.now() ) {
                animator.SetTrigger("Damaged");
                status.invincible.enable(60);
                status.hp -= dam;
                Debug.Log("damaged!");
            }
        }

        public void freeze(bool flag=true) {
            status.freeze = flag;
        }

        public void respawn() {
            Application.LoadLevel("testplay");
        }

        void onTurnMiddle() {
            Debug.Log("on turn middle");
            turn_dir_switched = true;
        }

        void onTurnEnd() {
            turn_dir_switched = false;
            animator.SetBool("Turn", false);
        }

        void OnJumpAnimEnd() {
            // end of jump motion
            Debug.Log("on jump anim end");
            animator.SetBool("Jump", false);
            //animator.speed = 0.0f;	// slow animation
        }

        public bool isGrounded() {
            return Physics.Raycast(transform.position + ground_raycast_offset, Vector3.down, 0.5f) ||
                Physics.Raycast(transform.position + ground_raycast_offset, new Vector3(1.0f, 0.0f, 0), 1.0f) ||
                Physics.Raycast(transform.position + ground_raycast_offset, new Vector3(-1.0f, 0.0f, 0), 1.0f);
        }

        public bool isLookAhead() {
            return transform.forward.x >= 1.0f;
        }

        public bool isLookBack() {
            return transform.forward.x <= -1.0f;
        }

        public bool isTurnDirSwitched() {
            return turn_dir_switched;
        }

        public bool isDash() {
            return ((PlayerDash)action_manager.getAction("DASH")).isDash();
        }

        public bool isAnimState(string anim_name) {
            AnimatorStateInfo anim_state = animator.GetCurrentAnimatorStateInfo(0);
            return anim_state.nameHash == Animator.StringToHash(anim_name);
        }

        public PlayerCamera getPlayerCamera() {
            return camera;
        }

        public Animator getAnimator() {
            return animator;
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

        public PlayerActionManager getActionManager() {
            return action_manager;
        }

        public void setAreaName(string name) {
            area_name = name;
        }

        public string getAreaName() {
            return area_name;
        }

        private void performTest() {
            Debug.Log("performTest");
        }


        void OnGUI() {
            Vector3 fw = transform.forward;
            Quaternion rot = transform.rotation;
            Vector3 targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            float vx = GetComponent<Rigidbody>().velocity.x;
            float vy = GetComponent<Rigidbody>().velocity.y;
            CapsuleCollider cc = GetComponent<CapsuleCollider>();
            GUI.Box(new Rect(Screen.width - 260, 10, 250, 250), "Interaction");
            GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "forward: " + fw);
            GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "vx: " + vx);
            GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "vy: " + vy);
            GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "targetDirection: " + targetDirection);
            GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "on_ground: " + isGrounded());
            GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "touched_wall: " + isTouchedWall());
            GUI.Label(new Rect(Screen.width - 245, 150, 250, 30), "(x,y,z): " + transform.position);
            GUI.Label(new Rect(Screen.width - 245, 170, 250, 30), "capsule_center: " + cc.bounds.center);
            GUI.Label(new Rect(Screen.width - 245, 190, 250, 30), "capsule_height: " + cc.height);
            GUI.Label(new Rect(Screen.width - 245, 210, 250, 30), "areaname: " + area_name);
            GUI.Label(new Rect(Screen.width - 245, 230, 250, 30), "animspeed: " + animator.speed);
        }
    }
}

/*
 Unitychan Animation

unitychan_JUMP00
	start: 10 - end: 56
	Root Transform Position (Y) : off
	Events:
		* 0.8 : OnJumpAnimEnd()

unitychan_SLIDE00
    start: 15 - end: 42

unitychan_UMATOBI00
    Root Transform Position (Y) : off
    start: 9 - end: 38

DefaultAvatar_PlantNTurn180
	start:151 - end: 194.7
	Root Transform Rotation : off
	Root Transform Position (Y) : on
	Root Transform Position (XZ) : off

Animator setting
    Apply root motion : false
    Update Mode : Normal
    Culling Mode : Based on renderers
*/

/*
 Rigidbody
    Mass: 30
    Drag: 0.2
    AngularDrag: 0.05
*/