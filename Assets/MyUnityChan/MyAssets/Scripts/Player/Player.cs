using UnityEngine;
using System.Collections;
using UnityChan;

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using RootMotion.FinalIK;

namespace MyUnityChan {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GroundChecker))]
    [RequireComponent(typeof(RoofChecker))]
    [RequireComponent(typeof(WallChecker))]
    public class Player : Character, ICharacterWalk, ICharacterFootstep {

        [SerializeField]
        public PlayerCameraPosition player_camera_position;

        private GameObject player_root;
        private Animator animator;

        private float dist_to_ground;
        private Vector3 dist_checksphere_center = new Vector3(0, 0.6f, 0);
        private Vector3 ground_raycast_offset = new Vector3(0, 0.05f, 0);
        private float anim_speed_default;
        private bool turn_dir_switched = false;

        private const float CHECKSPHERE_RADIUS = 0.1f;	// radius of sphere to check player is on ground
        private GameObject sphere_ground_check = null;

        public Vector3 last_entrypoint { get; set; }    // player's position in last area change

        public PlayerManager manager { get; set; }
        public PlayerActionManager action_manager { get; set; }
        public List<Const.BeamName> beam_slot { get; set; }
        public WallChecker wall_checker { get; set; }
        public UnityChanBoneManager bone_manager { get; set; }
        public BeamTurret beam_turret { get; protected set; }
        public FullBodyBipedIK ik { get; protected set; }

        public bool playable {
            get {
                return manager.playable;
            }
        }

        public string player_name {
            get {
                return manager.player_name;
            }
        }

        // Awake
        protected override void awake() {
            // animation
            animator = GetComponent<Animator>();
            bone_manager = GetComponent<UnityChanBoneManager>();
            ik = GetComponent<FullBodyBipedIK>();
            beam_turret = GetComponent<BeamTurret>();
            beam_slot = new List<Const.BeamName>();
        }

        // Start
        protected override void start() {
            player_root = transform.parent.gameObject;

            anim_speed_default = animator.speed * 1.2f;
            dist_to_ground = GetComponent<CapsuleCollider>().height;
            wall_checker = GetComponent<WallChecker>();

            // init player actions (required)
            registerActions(new List<Const.PlayerAction>{
                Const.PlayerAction.ACCEL, Const.PlayerAction.BRAKE, Const.PlayerAction.DOWN,
                Const.PlayerAction.JUMP, Const.PlayerAction.LIMIT_SPEED, Const.PlayerAction.TURN,
                Const.PlayerAction.SWITCH_BEAM, Const.PlayerAction.WALL_JUMP,
                Const.PlayerAction.TRANSFORM
            });

            // init sound player
            setupSoundPlayer();

            //player = gameObject;
            position_history = new RingBuffer<Vector3>(10);

            if ( playable ) {
                // player infomation for NPC
                NPCharacter.setPlayers();
            }

        }

        protected override void update() {
            float vy = rigid_body.velocity.y;

            if ( vy <= 0 && isGrounded() ) {
                // landing
                animator.SetBool("OnGround", true);
                animator.speed = anim_speed_default;
                animator.SetBool("Jump", false);
            }

            updateStunned();
            recordPosition();

            if ( controller.keyTest() ) {
                performTest();
            }
        }

        public void registerAction(Const.PlayerAction action_class) {
            if ( !action_manager ) action_manager = GetComponent<PlayerActionManager>();

            switch ( action_class ) {
                case Const.PlayerAction.ACCEL:
                    action_manager.registerAction(new PlayerAccel(this)); break;
                case Const.PlayerAction.ATTACK:
                    action_manager.registerAction(new PlayerAttack(this)); break;
                case Const.PlayerAction.BEAM:
                    action_manager.registerAction(new PlayerBeam(this)); break;
                case Const.PlayerAction.BOMB:
                    action_manager.registerAction(new PlayerBomb(this)); break;
                case Const.PlayerAction.BRAKE:
                    action_manager.registerAction(new PlayerBrake(this)); break;
                case Const.PlayerAction.DASH:
                    action_manager.registerAction(new PlayerDash(this)); break;
                case Const.PlayerAction.DOWN:
                    action_manager.registerAction(new PlayerDown(this)); break;
                case Const.PlayerAction.DOUBLE_JUMP:
                    action_manager.registerAction(new PlayerDoubleJump(this)); break;
                case Const.PlayerAction.WALL_JUMP:
                    action_manager.registerAction(new PlayerWallJump(this)); break;
                case Const.PlayerAction.GUARD:
                    action_manager.registerAction(new PlayerGuard(this)); break;
                case Const.PlayerAction.SWITCH_BEAM:
                    action_manager.registerAction(new PlayerSwitchBeam(this)); break;
                case Const.PlayerAction.GRAPPLE:
                    action_manager.registerAction(new PlayerGrapple(this)); break;
                case Const.PlayerAction.HADOUKEN:
                    action_manager.registerAction(new PlayerHadouken(this)); break;
                case Const.PlayerAction.JUMP:
                    action_manager.registerAction(new PlayerJump(this)); break;
                case Const.PlayerAction.LIMIT_SPEED:
                    action_manager.registerAction(new PlayerLimitSpeed(this)); break;
                case Const.PlayerAction.MISSILE:
                    action_manager.registerAction(new PlayerMissile(this)); break;
                case Const.PlayerAction.SLIDING:
                    action_manager.registerAction(new PlayerSliding(this)); break;
                case Const.PlayerAction.TRANSFORM:
                    action_manager.registerAction(new PlayerTransform(this)); break;
                case Const.PlayerAction.TURN:
                    action_manager.registerAction(new PlayerTurn(this)); break;
                default:
                    Debug.LogWarning("Undefined player action: id=" + action_class);
                    break;
            }
        }

        public void registerActions(List<Const.PlayerAction> action_class_list) {
            action_class_list.ForEach(ac => registerAction(ac));
        }

        public void disableAction(string name) {
            action_manager.disableAction(name);
        }

        public void enableAction(string name) {
            action_manager.enableAction(name);
        }

        public void disableAction(Const.PlayerAction id) {
            action_manager.disableAction(id);
        }

        public void enableAction(Const.PlayerAction id) {
            action_manager.enableAction(id);
        }

        public override void damage(int dam) {
            if ( status.invincible.now() ) return;
            if ( isGuarding() ) {
                // Guard effect
                EffectManager.createEffect(Const.ID.Effect.GUARD_01, transform.position, 40, true);

                // Reaction force
                rigid_body.AddForce(transform.forward * (-10.0f), ForceMode.VelocityChange);
                return;
            }
            status.invincible.enable(10);
            status.hp -= dam;
            voice(Const.ID.PlayerVoice.DAMAGED);
            if ( playable ) {
                manager.camera.shake();
                manager.hpgauge.shake();
            }
        }

        public override void knockback(int dam) {
            if ( dam < 10 )
                return;

            if ( dam < 30 ) {
                lockInput(50);
                animator.SetTrigger("Damaged");
                return;
            }

            if ( dam < 60 ) {
                animator.CrossFade("Down", 0.2f);
                lockInput(80);
            }
        }

        public override bool isTouchedWall() {
            return wall_checker.isTouchedWall();
        }

        public void freeze(bool flag = true, int frame = 0) {
            if ( frame > 0 ) {
                Observable.TimerFrame(frame)
                    .Subscribe(_ => status.freeze = flag);
            }
            else {
                status.freeze = flag;
            }
        }

        public void respawn() {
            Application.LoadLevel("testplay");
        }

        public void voice(Const.ID.PlayerVoice voice_id, bool playOneShot=true, int delay = 0) {
            AudioClip clip = AssetBundleManager.get(
                Const.ID.AssetBundle.UNITYCHAN_VOICE, Const.Sound.Voice.UnityChan[voice_id]) as AudioClip;
            getSoundPlayer().play(clip, playOneShot, delay);
        }

        public void onGetPowerupItem() {
            AudioClip clip = AssetBundleManager.get(
                Const.ID.AssetBundle.UNITYCHAN_VOICE,
                Const.Sound.Voice.UnityChan[Const.ID.PlayerVoice.POWER_UP]) as AudioClip;
            getSoundPlayer().play(clip);
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

        public override bool isGrounded() {
            return ground_checker.isGrounded();
        }

        public bool isTurnDirSwitched() {
            return turn_dir_switched;
        }

        public bool isDash() {
            PlayerDash dash = ((PlayerDash)action_manager.getAction("DASH"));
            if ( dash == null ) return false;
            return dash.isDash();
        }

        public bool isGuarding() {
            PlayerGuard guard = ((PlayerGuard)action_manager.getAction("GUARD"));
            if ( guard == null ) return false;
            return guard.guarding;
        }

        public bool isGrappling() {
            PlayerGrapple grapple = action_manager.getAction<PlayerGrapple>("GRAPPLE");
            if ( grapple == null ) return false;
            return grapple.grappled;
        }

        public bool isAnimState(string anim_name) {
            // anim_name is an animator state name starts with "Base Layer." instead of clip name
            AnimatorStateInfo anim_state = animator.GetCurrentAnimatorStateInfo(0);
            return anim_state.nameHash == Animator.StringToHash(anim_name);
        }

        public PlayerCamera getPlayerCamera() {
            return manager.camera;
        }

        public override string getAreaName() {
            return manager.area_name;
        }

        public override void setAreaName(string name) {
            manager.area_name = name;
        }

        public void setController(Controller _controller) {
            controller = _controller;
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

        public override int getReservedHP() {
            return (status as PlayerStatus).reserved_hp;
        }

        public void interrupt() {
            lockInput(0);
        }

        public void resume() {
            unlockInput();
        }

        public void comeback(Vector3 dst) {
            interrupt();
            manager.camera.fadeOut(Const.Frame.PLAYER_COMEBACK_FADE);
            delay(40, () => {
                StartCoroutine(doComeback(dst));
                manager.camera.fadeIn(Const.Frame.PLAYER_COMEBACK_FADE);
            });
        }

        private IEnumerator doComeback(Vector3 dst) {
            Debug.Log("doComeback");
            transform.position = dst;
            RaycastHit ground;
            Physics.Raycast(transform.position, Vector3.down, out ground, 5.0f);
            EffectManager.createEffect(Const.ID.Effect.RESURRECTION_01, ground.point, 240, false);
            yield return new WaitForSeconds(0.5f);
            resume();
        }

        public void enableSpringManager() {
            GetComponent<SpringManager>().enabled = true;
        }

        public void disableSpringManager() {
            GetComponent<SpringManager>().enabled = false;
        }

        public void onFootstep(Const.ID.FieldType fieldtype) {
            doPrevInterval("footsteps", 20, () => {
                sound.play(Const.ID.SE.FOOTSTEP_1);
            });
        }

        public void onForward() {
            doPrevInterval("walk puff", 40, () => {
                EffectManager.createEffect(
                    Const.ID.Effect.DASH_SMOKE_PUFF,
                    transform.position.add(0.0f, 0.2f, 0.0f),
                    60, true);
            });
        }

        public void onStay() {
        }

        private void performTest() {
            Debug.Log("performTest");
        }

        /*
        public void OnCollisionStay(Collision collisionInfo) {
            DebugManager.log("tag=" + collisionInfo.gameObject.tag);
            if ( collisionInfo.gameObject.tag == "Ground" ) {
                var p = collisionInfo.contacts.OrderBy(cp => cp.point.y).First();
                foreach (var c in collisionInfo.contacts ) {
                    DebugManager.log(c.point);
                }
                if ( Mathf.Abs(height / 2.0f - Vector3.Distance(p.point, transform.position)) < 0.1f ) {
                    grounded = true;
                }
                DebugManager.log("cp=" + p.point);
                DebugManager.log("p=" + transform.position);
                DebugManager.log("dist=" + Vector3.Distance(p.point, transform.position));
                //DebugManager.self().drawLine(p.point, transform.position);
            }
        }

        public void OnCollisionExit(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Ground" ) {
                if ( collisionInfo.contacts.Length == 0 ) {
                    grounded = false;
                    return;
                }
                var p = collisionInfo.contacts.OrderBy(cp => cp.point.y).First();
                if ( Mathf.Abs(height / 2.0f - Vector3.Distance(p.point, transform.position)) >= 0.1f ) {
                    grounded = false;
                }
            }
        }
        */

        void OnGUI() {
            if ( !SettingManager.get(Settings.Flag.SHOW_DEBUG_WINDOW) ) {
                return;
            }

            Vector3 fw = transform.forward;
            Quaternion rot = transform.rotation;
            CapsuleCollider cc = GetComponent<CapsuleCollider>();
            GUI.Box(new Rect(Screen.width - 260, 10, 250, 300), "Interaction");
            GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "forward: " + fw);
            GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "vx: " + getVx());
            GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "vy: " + getVx());
            GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "on_ground: " + isGrounded());
            GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "is_hit_roof: " + isHitRoof());
            GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "touched_wall: " + isTouchedWall());
            GUI.Label(new Rect(Screen.width - 245, 150, 250, 30), "(x,y,z): " + transform.position);
            GUI.Label(new Rect(Screen.width - 245, 170, 250, 30), "capsule_center: " + cc.bounds.center);
            GUI.Label(new Rect(Screen.width - 245, 190, 250, 30), "capsule_height: " + cc.height);
            GUI.Label(new Rect(Screen.width - 245, 210, 250, 30), "areaname: " + getAreaName());
            GUI.Label(new Rect(Screen.width - 245, 230, 250, 30), "animspeed: " + animator.speed);
            GUI.Label(new Rect(Screen.width - 245, 250, 250, 30), "focus ui: " + MenuManager.getCurrentSelectedName());
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
