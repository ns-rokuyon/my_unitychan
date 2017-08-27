using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    [System.Serializable]
    public class PhysicsSpec : IAttackSpec, ISoundSpec, IEffectSpec {
        [SerializeField] public string name;
        [SerializeField] public float damage_scale = 0;  
        [SerializeField] public int stun = 0;      // time enemy is stuned
        [SerializeField] public int hitstop = 0;      // hitstop frames
        [SerializeField] public int knockback = 0;      // knockback
        [SerializeField] public bool multiply_energy_and_damage; // Damage will be calculated based on energy * damage_scale
        [SerializeField] public float min_energy_threshold;     // When the energy is less than this threshold, attack() does nothing at all

        [SerializeField] public Const.ID.SE ground_hit_se = Const.ID.SE._NO;
        [SerializeField] public Const.ID.Effect hit_effect = Const.ID.Effect._NO_EFFECT;

        public PhysicsObject self { get; set; }

        public void attack(Character character, Hitbox hitbox = null) {
            if ( self.kinetic_energy < min_energy_threshold )
                return;

            if ( character ) {
                int damage;
                if ( multiply_energy_and_damage ) {
                    damage = (int)(damage_scale * self.kinetic_energy);
                }
                else {
                    damage = (int)damage_scale;
                }
                character.stun(stun);
                character.damage(damage);
                character.knockback(knockback);
                character.hitstop(hitstop);
            }
        }

        public void playSound(ObjectBase o = null, Hitbox hitbox = null) {
            if ( o )
                o.se(ground_hit_se);
            if ( self )
                self.se(ground_hit_se);
        }

        public void playEffect(ObjectBase o = null, Hitbox hitbox = null) {
            if ( o && o is Character ) {
                EffectManager.createEffect(hit_effect, (o as Character), 0, 0, 60);
                return;
            }
            EffectManager.createEffect(hit_effect, self.transform.position, 60);
        }
    }
}