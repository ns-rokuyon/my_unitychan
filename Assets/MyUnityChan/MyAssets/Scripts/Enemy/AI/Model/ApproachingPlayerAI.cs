using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class ApproachingPlayerAI : AIModel {
        public bool allow_attack;
        public float close_range;

        public override AI define() {
            return AI.root(self, controller)
                .def(AI.Def.Pattern.InputHorizontalTowardPlayer(this))
                .def(AI.Def.Name("Check allow attack")
                     .StopIf(_ => !allow_attack))
                .def(AI.Def.Name("Attack")
                     .If(s => self.distanceXTo(s.player) <= close_range)
                     .Then(_ => controller.inputKey(Controller.InputCode.ATTACK)));
        }
    }
}
