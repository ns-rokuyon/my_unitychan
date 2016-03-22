using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class AttackSpec {
        [SerializeField] public int damage;
        [SerializeField] public int stun;      // time enemy is stuned
        [SerializeField] public int frame;      // time hitbox enabled
        [SerializeField] public string effect_name;

        public virtual void attack(Character character, Hitbox hitbox) {
            if ( character ) {
                character.stun(stun);
                character.damage(damage);
            }

            if ( effect_name.Length != 0 ) {
                EffectManager.self().createEffect(Const.Prefab.Effect[effect_name],
                    hitbox.gameObject.transform.position, 60, true);
            }
        }
    }
}