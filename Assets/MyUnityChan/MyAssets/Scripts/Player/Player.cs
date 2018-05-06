using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityChan;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using RootMotion.FinalIK;

namespace MyUnityChan {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GroundChecker))]
    [RequireComponent(typeof(RoofChecker))]
    [RequireComponent(typeof(WallChecker))]
    [RequireComponent(typeof(PlayerIK))]
    public class Player : Character, ICharacterWalk, ICharacterFootstep, ICharacterMissileTankOwnable {

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
        public bool is_rolling { get; set; }

        public PlayerManager manager { get; set; }
        public PlayerActionManager action_manager { get; set; }
        public WallChecker wall_checker { get; set; }
        public UnityChanBoneManager bone_manager { get; set; }
        public BeamTurret beam_turret { get; protected set; }
        public Bomber bomber { get; protected set; }
        public MissilePod missile_pod { get; protected set; }
        public PlayerIK ik { get; protected set; }
        public Weapon weapon { get; set; }
        public CapsuleCollider collider { get; protected set; }
        public Equipment equipment { get; protected set; }
        public float ground_distance { get; private set; }

        // Available missiles
        public List<Const.ID.Projectile.Missile> missile_slot { get; set; }

        // Available bombs
        public List<Const.ID.Bomb> bomb_slot { get; set; }

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

        public ReadOnlyReactiveProperty<int> MissileTankNumStream {
            get { return manager.status.MissileTankNumStream; }
        }

        // Awake
        protected override void awake() {
            // animation
            animator = GetComponent<Animator>();
            bone_manager = GetComponent<UnityChanBoneManager>();
            ik = GetComponent<PlayerIK>();
            beam_turret = GetComponent<BeamTurret>();
            missile_pod = GetComponent<MissilePod>();
            bomber = GetComponent<Bomber>();
            collider = GetComponent<CapsuleCollider>();
            equipment = GetComponent<Equipment>();

            if ( missile_slot == null )
                missile_slot = new List<Const.ID.Projectile.Missile>();
            if ( bomb_slot == null )
                bomb_slot = new List<Const.ID.Bomb>();
        }

        // Start
        protected override void start() {
            player_root = transform.parent.gameObject;

            anim_speed_default = animator.speed * 1.2f;
            dist_to_ground = GetComponent<CapsuleCollider>().height;
            wall_checker = GetComponent<WallChecker>();

            // Init player actions (required)
            registerActions(Const.PlayerCommonDefaultActions);

            // Init player actions (optional)
            registerActions(Const.PlayerDefaultActions[character_name]);

            // init sound player
            setupSoundPlayer();

            position_history = new RingBuffer<Vector3>(10);

            this.UpdateAsObservable()
                .Where(_ => gameObject.activeSelf)
                .Where(_ => ground_checker)
                .Subscribe(_ => ground_distance = ground_checker.getDistance())
                .AddTo(this);

            if ( playable ) {
                // player infomation for NPC
                NPCharacter.setPlayers();
            }
        }

        protected override void update() {
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
                case Const.PlayerAction.JUMP:
                    action_manager.registerAction(new PlayerJump(this)); break;
                case Const.PlayerAction.LIMIT_SPEED:
                    action_manager.registerAction(new PlayerLimitSpeed(this)); break;
                case Const.PlayerAction.MISSILE:
                    action_manager.registerAction(new PlayerMissile(this)); break;
                case Const.PlayerAction.TRANSFORM:
                    action_manager.registerAction(new PlayerTransform(this)); break;
                case Const.PlayerAction.TURN:
                    action_manager.registerAction(new PlayerTurn(this)); break;
                case Const.PlayerAction.PICKUP:
                    action_manager.registerAction(new PlayerPickup(this)); break;
                case Const.PlayerAction.THROW:
                    action_manager.registerAction(new PlayerThrow(this)); break;
                case Const.PlayerAction.DEAD:
                    action_manager.registerAction(new PlayerDead(this)); break;
                case Const.PlayerAction.ROLL_FORWARD:
                    action_manager.registerAction(new PlayerRollForward(this)); break;
                case Const.PlayerAction.ROLL_BACKWARD:
                    action_manager.registerAction(new PlayerRollBackward(this)); break;
                case Const.PlayerAction.FALL:
                    action_manager.registerAction(new PlayerFall(this)); break;
                case Const.PlayerAction.LAND:
                    action_manager.registerAction(new PlayerLand(this)); break;
                case Const.PlayerAction.STOP:
                    action_manager.registerAction(new PlayerStop(this)); break;
                default:
                    Debug.LogWarning("Undefined player action: id=" + action_class);
                    break;
            }
        }
        
        public void registerAttack(Const.ID.PlayerAttackType at, Const.ID.AttackSlotType st) {
            var attack = action_manager.getAction<PlayerAttack>("ATTACK");
            if ( attack == null )
                return;
            switch ( at ) {
                case Const.ID.PlayerAttackType.SLIDING:
                    attack.switchTo(new PlayerSliding(this), st);
                    break;
                default:
                    break;
            }
        }

        public void clearAttack(Const.ID.AttackSlotType st) {
            var attack = action_manager.getAction<PlayerAttack>("ATTACK");
            if ( attack == null )
                return;
            attack.clearSlot(st);
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

        public void addMissileTank() {
            manager.status.addMissileTank();
        }

        public override void damage(int dam) {
            if ( status.invincible.now() ) return;
            if ( manager.gameover ) return;
            if ( isGuarding() ) {
                // Damage to shield
                PlayerGuard guard = ((PlayerGuard)action_manager.getAction("GUARD"));
                guard.damage(dam);

                if ( guard.broken ) {
                    rigid_body.AddForce(transform.forward * (-20.0f), ForceMode.VelocityChange);
                }
                else {
                    // Successfully guarded
                    EffectManager.createEffect(Const.ID.Effect.GUARD_01, transform.position, 40, true);
                    rigid_body.AddForce(transform.forward * (-10.0f), ForceMode.VelocityChange);

                    return;
                }
            }
            status.invincible.enable(10);

            // Scaling
            dam = (int)(dam * manager.status.setting.ranges[Settings.Range.ENEMY_POWER_SCALE].value);

            // Decrease
            dam = Math.Max(dam - status.DEF, 0);

            // Apply
            status.hp -= dam;
            voice(Const.ID.PlayerVoice.DAMAGED);
            if ( playable ) {
                manager.camera.shake();
                manager.hpgauge.shake();
            }
        }

        public void cancelGuard() {
            PlayerGuard guard = action_manager.getAction<PlayerGuard>("GUARD");
            if ( guard != null )
                guard.cancel();
        }

        public override void knockback(int dam) {
            if ( status.invincible.now() ) return;
            if ( manager.gameover ) return;

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
            PlayerGrapple grapple = action_manager.getAction<PlayerGrapple>();
            if ( grapple == null ) return false;
            return grapple.grappled;
        }

        public bool isLanding() {
            PlayerLand land = action_manager.getAction<PlayerLand>();
            if ( land == null ) return false;
            return land.landing;
        }

        public bool isAttacking() {
            PlayerAttack attack = action_manager.getAction<PlayerAttack>();
            if ( attack == null ) return false;
            return attack.active_attack != Const.ID.AttackSlotType._NO || attack.transaction != null;
        }

        public bool isTriggering() {
            if ( !beam_turret )
                return false;
            return beam_turret.triggered;
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

        public void equip(IPickupable pickupable, Const.ID.PickupSlot slot) {
            if ( equipment )
                equipment.equip(pickupable, slot);
        }

        public InteractionObject unequip() {
            if ( equipment )
                return equipment.unequip();
            return null;
        }

        public bool hasEquipment() {
            if ( equipment )
                return !equipment.acceptable;
            return false;
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
            if ( !SettingManager.get(Settings.Flag.SHOW_DEBUG_WINDOW) )
                return;
            if ( !playable )
                return;

            PlayerAttack attack = action_manager.getAction<PlayerAttack>("ATTACK");
            Vector3 fw = transform.forward;
            Quaternion rot = transform.rotation;
            CapsuleCollider cc = GetComponent<CapsuleCollider>();
            GUI.Box(new Rect(Screen.width - 260, 10, 250, 330), "Debug Window");
            GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "forward: " + fw);
            GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "vx: " + getVx());
            GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "vy: " + getVy());
            GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "on ground: " + isGrounded());
            GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "touched roof: " + isHitRoof());
            GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "touched wall: " + isTouchedWall());
            GUI.Label(new Rect(Screen.width - 245, 150, 250, 30), "(x,y,z): " + transform.position);
            GUI.Label(new Rect(Screen.width - 245, 170, 250, 30), "capsule center: " + cc.bounds.center);
            GUI.Label(new Rect(Screen.width - 245, 190, 250, 30), "ground distance: " + ground_distance);
            GUI.Label(new Rect(Screen.width - 245, 210, 250, 30), "areaname: " + getAreaName());
            GUI.Label(new Rect(Screen.width - 245, 230, 250, 30), "fps: " + GameStateManager.approximatedFps);
            GUI.Label(new Rect(Screen.width - 245, 250, 250, 30), "focus ui(menu): " + MenuManager.getCurrentSelectedName());
            GUI.Label(new Rect(Screen.width - 245, 270, 250, 30), "focus ui: " + UIHelper.getCurrentSelectedUIObjectName());
            GUI.Label(new Rect(Screen.width - 245, 290, 250, 30), "gameover: " + manager.gameover);
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
