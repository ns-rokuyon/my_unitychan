using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class BreakableBlock : Block {
        public int hp;
        public Const.Name.Effect effect;

        public override void damage(int dam) {
            hp -= dam;
            if ( hp <= 0 ) broken();
        }

        public virtual void broken() {
            EffectManager.self().createEffect(effect, transform.position, 60, true);

            Destroy(gameObject);
        }
    }
}