using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ZakoAirTypeBase : Enemy {

        [System.Serializable]
        public class Param : StructBase {
            public Vector3 flyF;        // Force to fly forward
            public Vector3 flapF;       // Force to flap up
            public float max_speed;
            public int flap_interval;  
        };

        [SerializeField]
        public List<ZakoAirTypeBase.Param> default_params;

        public float flight_level { get; private set; }

        public ZakoAirTypeBase.Param param {
            get; protected set;
        }

        protected override void awake() {
            base.awake();
            flight_level = transform.position.y;
        }

        protected override void start() {
            base.start();

            param = default_params[level - 1];

            action_manager.registerAction(new EnemyFly(this, param.flyF, param.flapF, param.max_speed));
            action_manager.registerAction(new EnemyDead(this));

            setHP(max_hp);
        }
    }
}
