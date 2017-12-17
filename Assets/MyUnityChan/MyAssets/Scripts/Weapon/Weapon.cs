using UnityEngine;
using System;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using System.Collections;
using RootMotion.FinalIK;

namespace MyUnityChan {

    [Serializable]
    public class HandTarget : KV<Const.ID.PickupSlot, GameObject> {
        public HandTarget(Const.ID.PickupSlot slot, GameObject obj) : base(slot, obj) {
        }
    }

    [RequireComponent(typeof(InteractionObject))]
    public abstract class Weapon : ObjectBase, IPickupable {
        public SensorZone pickup_zone;
        public Collider physics_collider;
        public AttackHitbox hitbox;

        [SerializeField]
        public Const.ID.PickupSlot interaction_slot;   // slots for auto interaction

        [SerializeField]
        public List<HandTarget> follow_hand_targets;

        [SerializeField]
        public List<MeleeAttackSpec> specs;

        public bool can_pickup;
        public bool owned;
        public IDisposable attacking { get; protected set; }

        private FullBodyBipedIK ik;
        private float hold_weight;
        private InteractionObject iobj;
        private OffsetPose hold_pose;

        public bool canPickup {
            get { return can_pickup; }
        }

        public bool isOwned {
            get { return owned; }
        }

        public Vector3 position {
            get { return transform.position; }
        }

        public InteractionObject interaction_object {
            get { return iobj; }
        }

        public Const.ID.PickupSlot interactionSlot {
            get { return interaction_slot; }
        }

        public List<HandTarget> followHandTargets {
            get { return follow_hand_targets; }
        }

        public virtual void Awake() {
            attacking = null;
            iobj = GetComponent<InteractionObject>();
            hold_pose = GetComponent<OffsetPose>();

            if ( pickup_zone ) {
                pickup_zone.onPlayerEnterCallback = (Player p, Collider c) => { can_pickup = true; };
                pickup_zone.onPlayerExitCallback = (Player p, Collider c) => { can_pickup = false; };
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

        public virtual void onPickedUpBy(Character ch) {
            owned = true;
            can_pickup = false;
            rigid_body.isKinematic = true;
            rigid_body.useGravity = false;

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

            StartCoroutine(increaseHoldWeight());
        }

        public virtual void onThrownOutBy(Character ch, float throw_fx) {
            Player player = ch as Player;
            var attack = player.action_manager.getAction<PlayerAttack>("ATTACK");
            var pickup = player.action_manager.getAction<PlayerPickup>("PICKUP");

            // Remove a weapon reference from player
            player.weapon = null;

            // Set owned flag to false
            owned = false;
            can_pickup = false;

            // Reset attacks
            attack.resetToDefaultAttacks();

            // Move weapon object to area group in hierarchy
            transform.parent = player.parent_area.transform.parent;
            AreaManager.self().relabelObject(gameObject);

            // Enable physics collider
            delay(2, () => { physics_collider.gameObject.SetActive(true); });

            // Enable hitbox
            delay(4, () => {
                hitbox.setOwner(ch.gameObject);
                hitbox.persistent = true;
                hitbox.gameObject.SetActive(true);
            });

            // Enable pickup zone
            delay(30, () => { pickup_zone.gameObject.SetActive(true); });

            // Release current interaction
            if ( pickup != null ) {
                pickup.release(Const.ID.PickupSlot.LEFT_HAND);
                pickup.release(Const.ID.PickupSlot.RIGHT_HAND);
            }

            // Throwout this object
            rigid_body.isKinematic = false;
            rigid_body.useGravity = true;
            rigid_body.AddForce(
                new Vector3(player.getFrontVector().x * throw_fx, 10, 0), ForceMode.Impulse);

            // Disable hitbox when velocity is 0
            delay(4, () => {
                this.ObserveEveryValueChanged(_ => rigid_body.velocity.x)
                    .TakeWhile(x => x > 0.01f)
                    .Subscribe(x => {
                        DebugManager.log("vx=" + x);
                    }, () => {
                        hitbox.gameObject.SetActive(false);
                    })
                    .AddTo(this);
            });

            // Decrease hold_weight
            StartCoroutine(decreaseHoldWeight());
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

        protected MeleeAttackSpec getSpec(Const.ID.AttackSlotType slot) {
            return specs.First(s => s.slot == slot);
        }

        public IEnumerator increaseHoldWeight() {
            ik = iobj.lastUsedInteractionSystem.GetComponent<FullBodyBipedIK>();
            while ( hold_weight < 1f ) {
                hold_weight += time_control.deltaTime;
                yield return null;
            }
        }

        public IEnumerator decreaseHoldWeight() {
            while ( hold_weight > 0f ) {
                hold_weight -= time_control.deltaTime;
                yield return null;
            }
        }
    }
}