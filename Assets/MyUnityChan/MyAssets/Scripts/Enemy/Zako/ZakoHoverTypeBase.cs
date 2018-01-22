using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ZakoHoverTypeBase : Enemy {

        [System.Serializable]
        public class Param : StructBase {
            public float base_vx;
            public float base_vy;
        };

        [SerializeField]
        public ZakoHoverTypeBase.Param default_param;

        public ZakoHoverTypeBase.Param param {
            get; protected set;
        }

        protected override void start() {
            base.start();

            param = default_param;

            action_manager.registerAction(new EnemyKinematics(this, param.base_vx, param.base_vy));
            action_manager.registerAction(new EnemyTurn(this));
            action_manager.registerAction(new EnemyDead(this));

            setHP(max_hp);
        }
    }
}
