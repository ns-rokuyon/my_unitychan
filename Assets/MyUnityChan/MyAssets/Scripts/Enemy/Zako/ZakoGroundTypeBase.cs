using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ZakoGroundTypeBase : Enemy {

        [System.Serializable]
        public class Param : StructBase {
            public float walk_fx;       // Force to walk forward
            public float max_speed;
        };

        [SerializeField]
        public List<ZakoGroundTypeBase.Param> default_params;

        public ZakoGroundTypeBase.Param param {
            get; protected set;
        }

        protected override void start() {
            base.start();

            param = default_params[level - 1];

            action_manager.registerAction(new EnemyWalk(this, new Vector3(param.walk_fx, 0, 0), param.max_speed));
            action_manager.registerAction(new EnemyDead(this));

            setHP(max_hp);
        }
    }
}