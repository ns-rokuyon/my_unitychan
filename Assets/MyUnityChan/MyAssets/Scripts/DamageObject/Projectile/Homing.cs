using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public class Homing : Projectile.Custom {
        public float angle_per_sec;     // Rotation speed
        public int lockon_frame_delay;
        [SerializeField]
        public Const.ID.TARGETING_MODE mode;

        public GameObject target { get; protected set; }
        public Vector3 destination { get; protected set; }
        public bool locked { get; protected set; }
        public bool end_homing { get; set; }

        void Awake() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            if ( !target ) {
                if ( searchTarget() ) {
                    Observable.TimerFrame(lockon_frame_delay)
                        .Subscribe(_ => lockon());
                }
            }

            if ( !locked ) return;
            if ( end_homing ) return;

            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(destination.changeZ(transform.position.z) - transform.position);
            if ( Quaternion.Angle(from, to) < 5.0f ) {
                transform.rotation = to;
                end_homing = true;
            }
            else {
                transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime * angle_per_sec);
            }
        }

        public bool searchTarget() {
            switch ( mode ) {
                case Const.ID.TARGETING_MODE.TO_PLAYER:
                    target = GameStateManager.getPlayerObject();
                    break;
                // TODO
                default:
                    break;
            }
            if ( !target )
                return false;
            return true;
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
        }

        public override void finalize() {
        }
    }
}