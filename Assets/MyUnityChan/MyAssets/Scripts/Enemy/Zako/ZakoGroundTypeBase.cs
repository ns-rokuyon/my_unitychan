using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ZakoGroundTypeBase : Enemy {

        public float walk_fx;       // Force to walk forward
        public float max_speed;

        protected override void start() {
            base.start();

            action_manager.registerAction(new EnemyWalk(this, new Vector3(walk_fx, 0, 0), max_speed));
            action_manager.registerAction(new EnemyDead(this));

            setHP(max_hp);
        }
    }
}