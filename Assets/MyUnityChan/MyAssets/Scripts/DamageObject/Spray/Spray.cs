using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;

namespace MyUnityChan {
    public class Spray : ShootableObject {
        [SerializeField] public SpraySpec spec;

        public List<PSEmissionEaser> easers { get; protected set; }
        public DamageObjectHitbox hitbox { get; protected set; }

        public override void Awake() {
            base.Awake();

            easers = GetComponentsInChildren<PSEmissionEaser>().ToList();

            if ( has_hitbox_in_children )
                hitbox = GetComponentInChildren<DamageObjectHitbox>();
            else
                throw new NotImplementedException();
        }

        public override void finalize() {
        }

        public override void initialize() {
        }

        public void powerOn(float easing_time = 0.0f) {
            easers.ForEach(easer => easer.powerOn(easing_time));
        }

        public void powerOff(float easing_time = 0.0f) {
            easers.ForEach(easer => easer.powerOff(easing_time));
        }
    }
}