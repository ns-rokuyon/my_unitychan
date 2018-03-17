using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class ItemDropper : ObjectBase {
        [System.Serializable]
        public class Def : KP<GameObject> {
            public Def(GameObject o, float p) : base(o, p) {
            } 
        }

        [SerializeField]
        private int drop_attempts = 1;

        [SerializeField]
        private List<Def> drop_items;

        private readonly float r = 2.0f;

        public void drop() {
            for ( int i = 0; i < drop_attempts; i++ ) {
                GameObject prefab = GameStateManager.RNG.prob<Def, GameObject>(drop_items);
                if ( prefab == null )
                    continue;

                var shift_x = (GameStateManager.RNG.value - 0.5f) * r;
                var shift_y = (GameStateManager.RNG.value - 0.5f) * r;
                var item = DropItemManager.Instance.create<DropItem>(prefab, true);
                item.setPosition(transform.position + Vector3.up + shift_x * Vector3.right + shift_y * Vector3.up);
            }
        }
    }
}
