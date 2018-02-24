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
                HitboxManager.createHitbox<DamageObjectHitbox>(spec.hitbox_id);
            hitbox.ready(gameObject, spec);

            // Create effect
            EffectManager.createEffect(
                spec.effect_name,
                gameObject.transform.position,
                100,
                true);

            // Delete this object
            ObjectPoolManager.releaseGameObject(this);
        }

    }
}
