using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyBeam : EnemyActionBase {
        private BeamTurret shooter;

        public EnemyBeam(Character character) : base(character) {
            shooter = character.GetComponent<BeamTurret>();
        }

        public override bool condition() {
            return controller.keyProjectile();
        }

        public override string name() {
            return "BEAM";
        }

        public override void perform() {
            shooter.trigger();
        }

        public override void off_perform() {
            shooter.trigger(false);
        }
    }

}
