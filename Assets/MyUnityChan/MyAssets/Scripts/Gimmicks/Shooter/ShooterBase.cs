using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public abstract class ShooterBase : ObjectBase {
        public bool auto = false;           // Auto trigger in each shooting frame

        private bool triggered = false;     // Flag for specifying shooting frame manually
        protected bool shooting;
        protected SoundPlayer sound_player;
        protected Character owner;

        /*
           [ round=1 ]
            |-----interval_frame-----|-------------- -- -
          shoot                    shoot

           [ round=3 ]
            |--burst_delta--|--burst_delta--|-----interval_frame-----|--burst_delta--|-- -
          shoot           shoot           shoot                    shoot           shoot
        */
        protected int n_round_burst;
        protected int burst_delta_frame;
        protected int interval_frame;
        protected string hitbox_name;
        protected Const.ID.SE se_name;

        protected string projectile_name;

        void Start() {
            baseStart();
        }

        void Update() {
            baseUpdate();
        }

        // Angle of shooting
        public virtual Vector3 angle() {
            return new Vector3(1.0f, 0.0f, 0.0f);
        }

        public virtual void shoot() { }

        public virtual void sound() {
            if ( se_name == Const.ID.SE._NO ) {
                return;
            }
            if ( sound_player == null ) {
                sound_player = gameObject.GetComponent<SoundPlayer>();
            }
            if ( sound_player == null ) {
                return;
            }
            sound_player.play(se_name);
        }

        public void trigger(bool t=true) {
            triggered = t;
        }

        public void baseStart() {
            shooting = false;
            owner = GetComponent<Character>();
        }

        public void setProjectile(string name) {
            projectile_name = name;
            Projectile proj = (Resources.Load(Const.Prefab.Projectile[projectile_name]) as GameObject).GetComponent<Projectile>();
            ProjectileSpec spec = proj.spec;

            n_round_burst = spec.n_round_burst;
            burst_delta_frame = spec.burst_delta_frame;
            interval_frame = spec.interval_frame;
            se_name = spec.se_name;
            hitbox_name = spec.hitbox_name;
        }

        public void baseUpdate() {
            if ( owner.time_control.paused ) return;

            if ( owner != null && (owner.isFrozen() || owner.isStunned()) ) 
                return;
            
            if ( !auto && !triggered )
                return;

            if ( shooting )
                return;

            StartCoroutine(shootByTrigger());
        }

        public IEnumerator shootByTrigger() {
            shooting = true;
            if ( n_round_burst == 1 ) {
                shoot();
            }
            else {
                for ( int i = 0; i < n_round_burst; i++ ) {
                    shoot();
                    int delta = burst_delta_frame;
                    while ( delta > 0 ) {
                        delta--;
                        yield return null;
                    }
                }
            }
            int interval = interval_frame;
            while ( interval > 0 ) {
                interval--;
                yield return null;
            }
            shooting = false;
        }
    }
}
