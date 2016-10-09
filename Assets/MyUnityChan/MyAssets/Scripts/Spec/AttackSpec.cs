using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public abstract class AttackSpec {
        [SerializeField] public int damage;
        [SerializeField] public int stun;      // time enemy is stuned
        [SerializeField] public int frame;      // time hitbox enabled
        [SerializeField] public float launch_fy = 0.0f;
        [SerializeField] public Const.ID.SE hit_se = Const.ID.SE._NO;
        [SerializeField] public Const.ID.Effect effect_name;

        public virtual void attack(Character character, Hitbox hitbox) {
            if ( character ) {
                character.launch(launch_fy);
                character.stun(stun);
                character.damage(damage);
                character.se(hit_se);
                if ( character.getHP() <= 0 ) {
                    Character owner = hitbox.getOwner().GetComponent<Character>();
                    owner.defeatSomeone(character);
                }
            }

            if ( effect_name == Const.ID.Effect._NO_EFFECT )
                return;

            EffectManager.self().createEffect(
                effect_name,
                hitbox.gameObject.transform.position, 60, true);
        }
    }
}