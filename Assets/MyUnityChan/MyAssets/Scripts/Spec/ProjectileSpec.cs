using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class ProjectileSpec : AttackSpec {
        public int n_round_burst;
        public int burst_delta_frame;
        public int interval_frame;
        public string hitbox_name;
        [SerializeField] public Const.ID.SE se_name;
        [SerializeField] public Const.ID.Effect shoot_effect = Const.ID.Effect._NO_EFFECT;
    }
}
