using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class BombSpec : AttackSpec {
        [SerializeField]
        public Const.ID.Hitbox hitbox_id;

        [SerializeField]
        public string se_name;

        [SerializeField]
        public int stock_max;

        [SerializeField]
        public bool auto_reload;

        [SerializeField]
        public int reload_frame;
    }
}
