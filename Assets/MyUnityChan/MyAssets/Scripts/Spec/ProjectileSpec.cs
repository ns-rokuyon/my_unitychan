using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class ProjectileSpec : AttackSpec {
        public int shooting_frame;
        public int interval_frame;
        public string hitbox_name;
        [SerializeField]
        public Const.ID.SE.Projectile se_name;
    }
}
