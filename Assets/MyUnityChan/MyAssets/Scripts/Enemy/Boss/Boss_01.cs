using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Boss_01 : Boss {

        protected override void start() {
            base.start();

            action_manager.registerAction(new EnemyDead(this));
            action_manager.registerAction(new EnemyLookAtPlayer(this));
            action_manager.registerAction(new EnemyJump(this, 0.0f, 1.5f));
        }
    }
}
