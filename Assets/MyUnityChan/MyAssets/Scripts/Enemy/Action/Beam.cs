using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyBeam : EnemyActionBase {
        private ShooterBase shooter;

        public EnemyBeam(Character character) : base(character) {
            shooter = character.GetComponent<ShooterBase>();
        }

        public override bool condition() {
            return controller.keyProjectile() && !enemy.isStunned() && !enemy.isHitstopping();
        }

        public override string name() {
            return "BEAM";
        }

        public override void perform() {
            shooter.trigger();
            if ( enemy is IEnemyShoot ) {
                (enemy as IEnemyShoot).onShoot();
            }
        }

        public override void off_perform() {
            shooter.trigger(false);
        }
    }

}
