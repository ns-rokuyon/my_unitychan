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
        public bool easing;

        // Current instantiated spray component
        public Spray spray { get; protected set; }

        public Vector3 muzzle_point {
            get {
                return this.gameObject.transform.position + muzzle_offset;
            }
        }

        public override void Start() {
            base.Start();

            // Shooter
            this.ObserveEveryValueChanged(_ => triggered)
                .Subscribe(_ => {
                    if ( triggered && !spray ) {
                        shoot();
                    }
                    else if ( !triggered && spray ) {
                        if ( easing ) {
                            spray.powerOff(2.0f);
                            delay(120, () => {
                                ObjectPoolManager.releaseGameObject(spray.gameObject, Const.Prefab.Spray[spray_name]);
                                spray = null;
                            });
                        } else {
                            ObjectPoolManager.releaseGameObject(spray.gameObject, Const.Prefab.Spray[spray_name]);
                            spray = null;
                        }
                    }
                });

            // Rotater
            this.ObserveEveryValueChanged(_ => base_direction)
                .Where(_ => spray)
                .Subscribe(_ => {
                    // Revert to base angle (xdir=1)
                    spray.transform.localRotation = Quaternion.AngleAxis(0.0f, new Vector3(0, 0, 1));

                    // Rotate around z-axis
                    float ang = Mathf.Atan2(base_direction.y, base_direction.x) * Mathf.Rad2Deg;
                    spray.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), ang);
                });

            // Positioner
            this.UpdateAsObservable()
                .Where(_ => spray && owner)
                .Subscribe(_ => {
                    spray.transform.position = muzzle_point;
                });

        }

        public override void shoot() {
            if ( spray_name == Const.ID.Spray._NO_SET )
                return;

            if ( spray )
                return;

            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Spray[spray_name]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            spray = obj.GetComponent<Spray>();
            spray.time_control.changeClock(owner.time_control.clockName);
            spray.setStartPosition(muzzle_point);
            spray.linkShooter(this);
            spray.powerOn(2.0f);

            // hitbox
            DamageObjectHitbox hitbox;
            if ( spray.has_hitbox_in_children ) {
                hitbox = spray.gameObject.GetComponentInChildren<DamageObjectHitbox>();
                DebugManager.warn("hitbox pos = " + hitbox.transform.localPosition);
                hitbox.setEnabledCollider(true);
                hitbox.setOwner(gameObject);
                hitbox.ready(obj, spray.spec, keep_position: true);
                hitbox.depend_on_parent_object = true;
                DebugManager.warn("hitbox pos (after ready)= " + hitbox.transform.localPosition);
            }
            else {
                throw new NotImplementedException();
            }

            // sound
            sound();
        }
    }
}