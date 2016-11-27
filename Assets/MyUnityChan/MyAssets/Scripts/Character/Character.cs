using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Character : ObjectBase {
        // references to component
        protected Controller controller;

        // vars
        protected FrameTimerState inputlock_timer;
        protected RingBuffer<Vector3> position_history;

        public Const.CharacterName character_name { get; set; }
        public CharacterStatus status { get; set; }
        public GroundChecker ground_checker { get; protected set; }
        public float height { get; protected set; }
        public float width { get; protected set; }

        protected string area_name;
        protected int stunned = 0;

        protected virtual void awake() { }
        protected virtual void start() { }
        protected virtual void update() { }

        void Awake() {
            // Common Awake
            Renderer renderer = GetComponentInChildren<Renderer>();
            if ( renderer ) {
                Bounds bounds = renderer.bounds;
                height = bounds.size.y;
                width = bounds.size.x;
            }

            ground_checker = GetComponent<GroundChecker>();

            awake();
        }

        void Start() {
            start();
        }

        void Update() {
            update();
        }

        public Controller getController() {
            return controller;
        }

        public virtual bool isTouchedWall() {
            // check character is in front of wall
            //CapsuleCollider capsule_collider = GetComponent<CapsuleCollider>();
            Collider capsule_collider = GetComponent<Collider>();
            return Physics.Raycast(transform.position + new Vector3(0, capsule_collider.bounds.size.y, 0), transform.forward, 0.4f) ||
                Physics.Raycast(capsule_collider.bounds.center, transform.forward, 0.4f);
        }

        public int getHP() {
            return status.hp;
        }

        public void setHP(int _hp) {
            status.hp = _hp;
        }

        public virtual void setAreaName(string name) {
            area_name = name;
        }

        public virtual string getAreaName() {
            return area_name;
        }


        public bool isFrozen() {
            return status.freeze;
        }

        public virtual void stun(int stun_power) {
            stunned = stun_power;
        }

        public bool isStunned() {
            return stunned > 0 ? true : false;
        }

        protected void updateStunned() {
            if ( stunned > 0 ) {
                stunned--;
                clearPositionHistory();
            }
        }

        public virtual int getReservedHP() {
            return 0;
        }

        public virtual int getAllHP() {
            return getReservedHP() + getHP();
        }

        public virtual void damage(int dam) {
            if ( !status.invincible.now() ) {
                status.invincible.enable(30);
                status.hp -= dam;
            }
        }

        public virtual void launch(float power_y) {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if ( rigidbody ) {
                if ( isGrounded() )
                    rigidbody.AddForce(new Vector3(0.0f, power_y, 0.0f), ForceMode.Impulse);
                else 
                    rigidbody.velocity = Vector3.zero;
            }
        }

        public float distanceXTo(Character to) {
            return Mathf.Abs(to.transform.position.x - transform.position.x);
        }

        public float distanceYTo(Character to) {
            return Mathf.Abs(to.transform.position.y - transform.position.y);
        }

        public float getVx(bool abs=false) {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if ( rigidbody ) {
                if ( abs )
                    return Mathf.Abs(GetComponent<Rigidbody>().velocity.x);
                return GetComponent<Rigidbody>().velocity.x;
            }
            return 0.0f;    // TODO
        }

        public float getVy(bool abs=false) {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if ( rigidbody ) {
                if ( abs ) 
                    return Mathf.Abs(GetComponent<Rigidbody>().velocity.y);
                return GetComponent<Rigidbody>().velocity.y;
            }
            return 0.0f;
        }

        protected void recordPosition() {
            position_history.add(transform.position);
        }

        public int getPositionHistoryCount() {
            return position_history.count();
        }

        public void clearPositionHistory() {
            position_history.clear();
        }

        public Vector3 getPrevPosition(int prev) {
            if ( position_history == null )
                return transform.position;
            if ( prev >= getPositionHistoryCount() ) {
                return transform.position;
            }
            return position_history.getPrev(prev);
        }

        public Vector3 getOldestPosition() {
            if ( position_history == null )
                return transform.position;
            if ( getPositionHistoryCount() == 0 )
                return transform.position;
            return position_history.getLast();
        }

        public Vector3 getRecentTravelDistance() {
            Vector3 travel = Vector3.zero;
            Vector3 prev = Vector3.zero;
            int index = 0;
            foreach ( Vector3 pos in position_history ) {
                if ( index == 0 ) {
                    prev = pos;
                    index++;
                    continue;
                }
                travel = travel + new Vector3(Mathf.Abs(prev.x - pos.x), Mathf.Abs(prev.y - pos.y), 0.0f);
                index++;
                prev = pos;
            }
            return travel;
        }

        public virtual bool isGrounded() {
            throw new System.NotImplementedException();
        }

        public virtual void defeatSomeone(Character character) {
            // Callback method.
        }

        public bool isLookAhead() {
            return transform.forward.x >= 1.0f;
        }

        public bool isLookBack() {
            return transform.forward.x <= -1.0f;
        }

        public Vector3 getFrontVector() {
            if ( isLookAhead() )
                return Vector3.right;
            else
                return Vector3.left;
        }

        public Vector3 getBackVector() {
            return -1.0f * getFrontVector();
        }

        public void lookBack() {
            transform.LookAt(transform.position + getBackVector());
        }

        // xdir = 1.0f | -1.0f
        public void lookAtDirectionX(float xdir) {
            transform.LookAt(
                new Vector3(
                    transform.position.x + xdir * 100.0f, 
                    transform.position.y, 
                    transform.position.z
                )
            );
        }

        public void unlockInput() {
            if ( isInputLocked() ) inputlock_timer.destroy();
        }

        public void lockInput(int frame) {
            // disable movement by inputs for N frames specified
            // If frame is 0, endless dummy timer is created
            inputlock_timer.destroy();
            inputlock_timer.createTimer(frame);
        }

        public bool isInputLocked() {
            // return true when inputs are locked
            if ( inputlock_timer == null ) {
                Debug.LogWarning("inputlock_timer is not initialized");
                return false;
            }
            return inputlock_timer.isRunning();
        }


    }
}
