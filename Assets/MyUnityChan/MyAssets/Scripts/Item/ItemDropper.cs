using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ItemDropper : ObjectBase {
        [System.Serializable]
        public class Def : KP<DropItem> {
            public Def(DropItem o, float p) : base(o, p) {
            } 
        }

        [SerializeField]
        public List<Def> drop_items;

        public void drop() {
        }

        private void createItem() {
        }
    }
}
