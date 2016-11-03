using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class ShooterBase : ObjectBase {
        public bool auto = false;           // Auto trigger in each shooting frame
        private bool triggered = false;     // Flag for specifying shooting frame manually

        protected int start_frame;
        protected int frame_count;
        protected bool sleep;
        protected int sleep_start_frame;

        protected SoundPlayer sound_player;
        protected Character owner;

        protected int shooting_frame;
        protected int interval_frame;
        protected string hitbox_name;
        protected Const.ID.SE se_name;

        protected string projectile_name;

        public Const.ID.TARGETING_MODE targeting_mode { get; set; }

        void Start() {
            targeting_mode = Const.ID.TARGETING_MODE._NO_TARGETING;
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
            sleep = false;
            frame_count = 0;
            owner = GetComponent<Character>();
        }

        public void setProjectile(string name) {
            projectile_name = name;
            Projectile proj = (Resources.Load(Const.Prefab.Projectile[projectile_name]) as GameObject).GetComponent<Projectile>();
            ProjectileSpec spec = proj.spec;

            Homing homing = proj.GetComponent<Homing>();
            if ( homing ) {
                targeting_mode = homing.mode;
            }
            else {
                targeting_mode = Const.ID.TARGETING_MODE._NO_TARGETING;
            }

            shooting_frame = spec.shooting_frame;
            interval_frame = spec.interval_frame;
            se_name = spec.se_name;
            hitbox_name = spec.hitbox_name;
        }

        public void baseUpdate() {
            //if ( !auto ) return;
            if ( PauseManager.isPausing() ) return;

            if ( sleep ) {
                if ( Time.frameCount - sleep_start_frame >= interval_frame ) {
                    sleep = false;
                }
                return;
            }

            if ( owner != null && (owner.isFrozen() || owner.isStunned()) ) {
                wait();
                return;
            }
            

            if ( !auto && !triggered ) {
                return;
            }

            if ( frame_count > 0 ) {
                if ( shooting_frame <= frame_count ) {
                    wait();
                    return;
                }
                shoot();
                frame_count++;
            }
            else {
                shoot();
                frame_count = 1;
            }
        }

        public void wait() {
            frame_count = 0;
            sleep = true;
            sleep_start_frame = Time.frameCount;
        }
    }
}
