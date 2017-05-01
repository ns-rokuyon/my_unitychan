using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class MeleeAttackSpec : AttackSpec {
        [SerializeField]
        public Const.ID.AttackLevel level;
    }
}