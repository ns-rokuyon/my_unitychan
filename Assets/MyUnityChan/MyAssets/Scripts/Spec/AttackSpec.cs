﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class AttackSpec {
        [SerializeField] public string name;
        [SerializeField] public int damage = 0;
        [SerializeField] public int stun = 0;      // time enemy is stuned
        [SerializeField] public int frame = 0;      // time hitbox enabled
        [SerializeField] public int hitstop = 0;      // hitstop frames
        [SerializeField] public int knockback = 0;      // knockback
        [SerializeField] public float launch_fy = 0.0f;
        [SerializeField] public Const.ID.SE hit_se = Const.ID.SE._NO;
        [SerializeField] public Const.ID.Effect effect_name;
        [SerializeField] public float force_power = 0;
        [SerializeField, Range(-90, 90)] public float force_degree = 0;

        [SerializeField, ReadOnly] public Vector3 _force_direction;
        
        public virtual Vector3 force_direction {
            get {
                _force_direction = Misc.degree2DirectionVector3(force_degree);
                return _force_direction;
            }
        }

        public virtual void prepare(Hitbox hitbox) {
        }

        public virtual void attack(Character character, Hitbox hitbox) {
            if ( character ) {
                Character owner = hitbox.getOwner<Character>();
                character.launch(launch_fy);
                character.stun(stun);
                character.damage(damage);
                character.knockback(knockback);
                character.hitstop(hitstop);
                if ( owner ) {
                    owner.hitstop(hitstop);
                    if ( character.getHP() <= 0 )
                        owner.defeatSomeone(character);
                }
                if ( character is NPCharacter && owner is Player ) {
                    (character as NPCharacter).clearTouchingCount(owner as Player);
                }
            }
        }

        public virtual void force(Rigidbody rb, Hitbox hitbox) {
            var F = force_power * force_direction;
            var owner = hitbox.getOwner<Character>();
            if ( owner && owner.isLookBack() )
                F = F.flipX();
            rb.AddForce(F, ForceMode.Impulse);
        }

        public virtual void playSound(ObjectBase o, Hitbox hitbox) {
            o.se(hit_se);
        }

        public virtual void playEffect(ObjectBase o, Hitbox hitbox) {
            if ( effect_name == Const.ID.Effect._NO_EFFECT )
                return;

            EffectManager.createEffect(
                effect_name,
                hitbox.gameObject.transform.position, 60, true);
        }
    }
}