using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

namespace MyUnityChan {
    public class Sprayer : ShooterBase {
        [SerializeField]
        public Const.ID.Spray spray_name;

        // Emission easing
        public bool easing = true;
        public float easing_duration = 2.0f;

        // The limitation of continuous shooting
        public int overheat_limit_frame = 0;

        // Waiting frame of canceling overheat
        public int cooldown_frame = 0;

        // Current instantiated spray component
        public Spray spray { get; protected set; }

        // Offset position
        public Vector3 muzzle_point {
            get {
                return this.gameObject.transform.position + muzzle_offset;
            }
        }

        // Current shooting frame counter
        public int continuous_shooting_frame;

        // Overheating
        public bool overheating { get; set; }

        // Waiting to release object
        public bool shutdowning { get; protected set; }

        public override void Start() {
            base.Start();

            // Shooter
            this.ObserveEveryValueChanged(_ => getTriggered())
                .Subscribe(t => {
                    if ( time_control.paused )
                        return;
                    if ( shutdowning )
                        return;

                    if ( t && !spray ) {
                        shoot();
                    }
                    else if ( !t && spray ) {
                        if ( easing ) {
                            shutdowning = true;
                            spray.powerOff(easing_duration);
                            spray.hitbox.setEnabledCollider(false);
                            delay((int)easing_duration * 60, () => {
                                ObjectPoolManager.releaseGameObject(spray);
                                spray = null;
                                shutdowning = false;
                            });
                        } else {
                            ObjectPoolManager.releaseGameObject(spray);
                            spray = null;
                        }
                    }
                })
                .AddTo(this);

            // Rotater
            this.ObserveEveryValueChanged(_ => base_direction)
                .Where(_ => spray)
                .Subscribe(_ => {
                    // Revert to base angle (xdir=1)
                    spray.transform.localRotation = Quaternion.AngleAxis(0.0f, new Vector3(0, 0, 1));

                    // Rotate around z-axis
                    float ang = Mathf.Atan2(base_direction.y, base_direction.x) * Mathf.Rad2Deg;
                    spray.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), ang);
                })
                .AddTo(this);

            // Positioner
            this.UpdateAsObservable()
                .Where(_ => spray)
                .Subscribe(_ => {
                    // Fix point
                    spray.transform.position = muzzle_point;

                    // Counter
                    if ( !overheating )
                        continuous_shooting_frame++;
                })
                .AddTo(this);

            // Overheat manager
            this.ObserveEveryValueChanged(_ => continuous_shooting_frame)
                .Where(frame => overheat_limit_frame > 0 && !overheating && frame >= overheat_limit_frame)
                .Subscribe(_ => {
                    if ( time_control.paused )
                        return;

                    overheating = true;

                    int cooldown = cooldown_frame + (int)(easing_duration * GameStateManager.fps);
                    delay(cooldown, () => {
                        overheating = false;
                        continuous_shooting_frame = 0;
                    });
                })
                .AddTo(this);
        }

        public override void shoot() {
            if ( spray_name == Const.ID.Spray._NO_SET )
                return;

            if ( overheating )
                return;

            if ( spray )
                return;

            GameObject obj = ObjectPoolManager.getGameObject(
                ConfigTableManager.Spray.getPrefabConfig(spray_name).prefab);

            obj.setParent(Hierarchy.Layout.PROJECTILE);

            spray = obj.GetComponent<Spray>();
            spray.time_control.changeClock(owner.time_control.clockName);
            spray.setStartPosition(muzzle_point);
            spray.linkShooter(this);
            spray.powerOn(easing_duration);

            // hitbox
            spray.hitbox.setEnabledCollider(true);
            spray.hitbox.setOwner(gameObject);
            spray.hitbox.ready(obj, spray.spec, keep_position: true);
            spray.hitbox.depend_on_parent_object = true;

            // sound
            sound();

            continuous_shooting_frame = 0;
        }

        public bool getTriggered() {
            return overheating ? false : triggered;
        }
    }
}