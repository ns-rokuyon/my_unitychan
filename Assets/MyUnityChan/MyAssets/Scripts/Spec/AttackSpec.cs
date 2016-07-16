using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class AttackSpec {
        [SerializeField] public int damage;
        [SerializeField] public int stun;      // time enemy is stuned
        [SerializeField] public int frame;      // time hitbox enabled
        [SerializeField] public Const.Name.Effect effect_name;

        public virtual void attack(Character character, Hitbox hitbox) {
            if ( character ) {
                character.stun(stun);
                character.damage(damage);
            }

            if ( effect_name == Const.Name.Effect._NO_EFFECT )
                return;

            EffectManager.self().createEffect(
                effect_name,
                hitbox.gameObject.transform.position, 60, true);
        }
    }
}