using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public abstract class Bomb : DamageObjectBase {

        [SerializeField]
        public BombSpec spec;

        public virtual void explode() {
            // Create hitbox
            DamageObjectHitbox hitbox = 
                HitboxManager.self().create<DamageObjectHitbox>(Const.Prefab.Hitbox[spec.hitbox_name], true);
            hitbox.ready(gameObject, spec);

            // Create effect
            EffectManager.self().createEffect(
                Const.Prefab.Effect[spec.effect_name],
                gameObject.transform.position,
                100,
                true);
        }
    }
}
