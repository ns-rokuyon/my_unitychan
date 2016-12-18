using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyAttack : EnemyActionBase {
        public EnemyAttack(Character character) : base(character) {
        }

        public override void perform() {
            if ( enemy is IEnemyAttack )
                (enemy as IEnemyAttack).attack();
        }

        public override bool condition() {
            return controller.keyAttack() && !enemy.isStunned() && !enemy.isHitstopping() && !enemy.isFrozen();
        }

        public override string name() {
            return "ATTACK";
        }
    }

}
