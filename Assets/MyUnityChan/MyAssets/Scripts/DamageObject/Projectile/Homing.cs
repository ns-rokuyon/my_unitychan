using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace MyUnityChan {
    public class Homing : Projectile.Custom {
        public float lerp_t;                // 0.0 - 1.0
        public int lockon_frame_delay;
        public bool lockon_once;            // If false, enable to update target position
        public float finish_range_radius;   // When the projectile reach the point, end_homing will be change to true;
        [SerializeField] public Const.ID.TARGETING_MODE mode;

        public GameObject target { get; protected set; }
        public Projectile projectile { get; set; }
        public Vector3 destination { get; protected set; }
        public bool locked { get; protected set; }
        public bool end_homing { get; set; }

        protected IDisposable targetter;
        protected IDisposable aimer;
        protected IDisposable rudder;

        void Awake() {
            initialize();
        }

        protected void homing() {
            Vector3 diff = destination - transform.position;
            float dist = Vector3.Distance(destination, transform.position);
            if ( dist < finish_range_radius ) {
                end_homing = true;
                return;
            }
            float target_angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            float now_angle = Mathf.Atan2(projectile.target_dir.y, projectile.target_dir.x) * Mathf.Rad2Deg;
            if ( Mathf.Abs(Mathf.DeltaAngle(now_angle, target_angle)) < 4.0f ) {
                end_homing = true;
                return;
            }
            float interpolation_angle = Mathf.LerpAngle(now_angle, target_angle, lerp_t) * Mathf.Deg2Rad;

            projectile.target_dir = new Vector3(
                Mathf.Cos(interpolation_angle),
                Mathf.Sin(interpolation_angle),
                0.0f);
        }

        public void searchTarget() {
            switch ( mode ) {
                case Const.ID.TARGETING_MODE.TO_PLAYER:
                    target = GameStateManager.getPlayerObject();
                    break;
                // TODO
                default:
                    break;
            }
        }

        public void setTarget(GameObject obj) {
            target = obj;
        }

        public void lockon() {
            destination = target.gameObject.transform.position;
            locked = true;
        }

        public override void initialize() {
            locked = false;
            target = null;
            end_homing = false;
            projectile = GetComponent<Projectile>();

            targetter = this.UpdateAsObservable()
                .Subscribe(_ => searchTarget());

            if ( lockon_once ) {
                aimer = this.UpdateAsObservable()
                    .Where(_ => target != null)
                    .DelayFrame(lockon_frame_delay)
                    .First()
                    .Subscribe(_ => {
                        lockon();
                    });
            }
            else {
                aimer = this.UpdateAsObservable()
                    .Where(_ => target != null)
                    .ThrottleFrame(lockon_frame_delay)
                    .Subscribe(_ => lockon());
            }

            rudder = this.UpdateAsObservable()
                .Where(_ => locked)
                .Where(_ => !end_homing)
                .Subscribe(_ => homing());
        }

        public override void finalize() {
            targetter.Dispose();
            aimer.Dispose();
            rudder.Dispose();
        }
    }
}