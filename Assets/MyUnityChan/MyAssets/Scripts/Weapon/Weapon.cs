﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace MyUnityChan {
    public abstract class Weapon : ObjectBase {
        public SensorZone pickup_zone;
        public Collider physics_collider;
        public AttackHitbox hitbox;

        [SerializeField]
        public List<MeleeAttackSpec> specs;

        protected bool is_can_pickup;
        protected bool is_owned;
        public IDisposable attacking { get; protected set; }

        public bool isCanPickup {
            get { return is_can_pickup; }
        }

        public bool isOwned {
            get { return is_owned; }
        }

        public virtual void Awake() {
            attacking = null;
            if ( pickup_zone ) {
                pickup_zone.onPlayerEnterCallback = () => { is_can_pickup = true; };
                pickup_zone.onPlayerExitCallback = () => { is_can_pickup = false; };
            }
        }

        public abstract void setAttackAction(Player player);

        public virtual void onAttack(int delay_frame, int frame, bool cancel_prev_attack,
                             Action<AttackHitbox, long> hitbox_updater = null, AttackSpec spec = null) {
            if ( cancel_prev_attack )
                cancelAttacking();

            if ( attacking != null )
                return;

            attacking = Observable.TimerFrame(0, 1)
                .SkipWhile(f => f < delay_frame)
                .Select(f => f - delay_frame)
                .Take(frame)
                .Subscribe(f => {
                    // Attacking frame
                    // f: 0 ~ frame-1
                    hitbox.gameObject.SetActive(true);  // Enable hitbox
                    if ( spec != null ) {
                        hitbox.spec = spec;
                    }
                    else {
                        if ( hitbox_updater == null ) {
                            DebugManager.warn("Hitbox spec has not been updated");
                        }
                        else {
                            hitbox_updater(hitbox, f);    // Report attacking frame count to updater
                        }
                    }
                }, () => {
                    // OnCompleted
                    attacking = null;
                    hitbox.gameObject.SetActive(false); // Disable hitbox
                })
                .AddTo(this);
        }

        public virtual void pickup(Character ch) {
            is_owned = true;
            Player player = (ch as Player);
            player.weapon = this;
            pickup_zone.gameObject.SetActive(false);
            delay(30, () => { physics_collider.gameObject.SetActive(false); });
            delay(30, () => {
                hitbox.setOwner(ch.gameObject);
                hitbox.persistent = true;
                hitbox.gameObject.SetActive(false);
            });
            delay(30, () => {
                setAttackAction(player);
            });
        }

        public virtual void cancelAttacking(int delay_frame = 0) {
            if ( delay_frame > 0 ) {
                delay(delay_frame, () => {
                    if ( attacking != null ) {
                        attacking.Dispose();
                        hitbox.gameObject.SetActive(false); // Disable hitbox
                        attacking = null;
                    }
                });
            }
            else {
                if ( attacking != null ) {
                    attacking.Dispose();
                    hitbox.gameObject.SetActive(false); // Disable hitbox
                    attacking = null;
                }
            }
        }

        protected MeleeAttackSpec getSpec(Const.ID.AttackLevel level) {
            return specs.First(s => s.level == level);
        }
    }
}