using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class ApproachingPlayerAI : AIModel {
        public bool allow_attack;
        public float close_range;

        public override AI define() {
            return AI.root(self, controller)
                .def(AI.Def.Name("Input forward")
                     .Keep(s => inputForward(s.player)))
                .def(AI.Def.Name("Check allow attack")
                     .StopIf(_ => !allow_attack))
                .def(AI.Def.Name("Attack")
                     .If(s => Mathf.Abs(s.player.transform.position.x - self.transform.position.x) <= close_range)
                     .Then(_ => controller.inputKey(Controller.InputCode.ATTACK)));
        }

        private void inputForward(Player player) {
            float target_x = player.getPrevPosition(6).x;
            float self_x = self.transform.position.x;

            if ( target_x < self_x ) {
                controller.inputHorizontal(-1.0f, 10);
            }
            else {
                controller.inputHorizontal(1.0f, 10);
            }
        }
    }
}
