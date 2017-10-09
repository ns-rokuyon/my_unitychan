using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

namespace MyUnityChan {
    public class Character : ObjectBase {
        // references to component
        protected Controller controller;

        // vars
        protected IDisposable locker;
        protected RingBuffer<Vector3> position_history;
        protected List<int> defeat_records;

        public Const.CharacterName character_name { get; set; }
        public GroundChecker ground_checker { get; protected set; }
        public RoofChecker roof_checker { get; protected set; }
        public float height { get; protected set; }
        public float width { get; protected set; }
        public virtual CharacterStatus status { get; set; }

        public int hitstop_counts { get; protected set; }
        public Vector3 hitstop_stock_velocity { get; protected set; }

        protected string area_name;
        protected int stunned = 0;

        protected virtual void awake() { }
        protected virtual void start() { }
        protected virtual void update() { }

        public void Awake() {
            // Common Awake
            Renderer renderer = GetComponentInChildren<Renderer>();
            if ( renderer ) {
                Bounds bounds = renderer.bounds;
                height = bounds.size.y;
                width = bounds.size.x;
            }

            ground_checker = GetComponent<GroundChecker>();
            roof_checker = GetComponent<RoofChecker>();
            defeat_records = new List<int>();
            locker = null;

            awake();
        }

        public void Start() {
            Observable.EveryUpdate()
                .Where(_ => hitstop_counts > 0)
                .Subscribe(_ => hitstop_counts--);

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

        public void hitstop(int frame) {
            if ( frame <= 0 )
                return;
            if ( !isHitstopping() ) {
                StartCoroutine(hitstopping(frame));
            }
            else {
                hitstop_counts = frame;
            }
        }

        protected IEnumerator hitstopping(int frame) {
            hitstop_counts = frame;
            var animator = GetComponent<Animator>();
            Vector3 v = rigid_body.velocity;
            DebugManager.log("keep=" + rigid_body.velocity);
            while ( isHitstopping() ) {
                rigid_body.velocity = Vector3.zero;
                if ( animator )
                    animator.speed = 0.1f;
                yield return null;
            }
            rigid_body.velocity = v;
            if ( animator )
                animator.speed = 1.0f;
        }

        public bool isHitstopping() {
            return hitstop_counts > 0;
        }

        public virtual bool isFlinching() {
            return isHitstopping() || isStunned();
        }

        public virtual int getReservedHP() {
            return 0;
        }

        public virtual int getAllHP() {
            return getReservedHP() + getHP();
        }

        public virtual void damage(int dam) {
            if ( !status.invincible.now() ) {
                status.invincible.enable(10);
                status.hp -= dam;
            }
        }

        public virtual void knockback(int dam) {
        }

        public virtual void launch(float power_y) {
            if ( power_y == 0.0f ) {
                return;
            }
            if ( isGrounded() )
                rigid_body.AddForce(new Vector3(0.0f, power_y, 0.0f), ForceMode.Impulse);
            else 
                rigid_body.velocity = Vector3.zero;
        }

        public void zeroVelocity() {
            rigid_body.velocity = Vector3.zero;
            rigid_body.angularVelocity = Vector3.zero;
        }

        public float distanceXTo(Character to) {
            return Mathf.Abs(to.transform.position.x - transform.position.x);
        }

        public float distanceXTo(Vector3 position) {
            return Mathf.Abs(position.x - transform.position.x);
        }

        public float distanceYTo(Character to) {
            return Mathf.Abs(to.transform.position.y - transform.position.y);
        }

        public float distanceYTo(Vector3 position) {
            return Mathf.Abs(position.y - transform.position.y);
        }

        public float getVx(bool abs=false) {
            if ( abs )
                return Mathf.Abs(rigid_body.velocity.x);
            return rigid_body.velocity.x;
        }

        public float getVy(bool abs=false) {
            if ( abs ) 
                return Mathf.Abs(rigid_body.velocity.y);
            return rigid_body.velocity.y;
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

        public Vector3 getLatestPositionDiff(bool in_lateupdate=false) {
            Vector3 diff = Vector3.zero;
            if ( in_lateupdate ) {
                if ( position_history.count() < 2 )
                    return diff;
                diff = position_history.getHead() - position_history.getPrev(1);
            }
            else {
                if ( position_history.count() < 1 )
                    return Vector3.zero;
                diff = transform.position - position_history.getHead();
            }
            return new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), 0);
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

        public virtual bool isHitRoof() {
            if ( roof_checker )
                return roof_checker.isHitRoof();
            return false;
        }

        public virtual void defeatSomeone(Character character) {
            defeat_records.Add(character.gameObject.GetInstanceID());
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

        public void flipDirection() {
            transform.LookAt(transform.position + getBackVector());
        }

        public void lookBack() {
            if ( isLookAhead() )
                flipDirection();
        }

        public void lookAhead() {
            if ( isLookBack() )
                flipDirection();
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
            if ( isInputLocked() ) {
                locker.Dispose();
                locker = null;
            }
        }

        public void lockInput(int frame) {
            // disable movement by inputs for N frames specified
            // If frame is 0, endless dummy timer is created
            if ( locker != null )
                unlockInput();

            if ( frame > 0 )
                locker = Observable.TimerFrame(frame)
                    .Subscribe(_ => unlockInput())
                    .AddTo(this);
            else
                locker = Observable.EveryUpdate()
                    .Subscribe(_ => { })
                    .AddTo(this);
        }

        public bool isInputLocked() {
            // return true when inputs are locked
            if ( locker == null )
                return false;
            return true;
        }
    }
}
