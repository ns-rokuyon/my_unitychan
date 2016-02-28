using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class ProjectileSpec : AttackSpec {
        public int shooting_frame;
        public int interval_frame;
        public string hitbox_name;
        public string se_name;

        public override void attack(Character character, Hitbox hitbox) {
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
