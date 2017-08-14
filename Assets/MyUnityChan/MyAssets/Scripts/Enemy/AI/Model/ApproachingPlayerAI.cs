using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class ApproachingPlayerAI : AIModel {
        public bool hover;
        public bool allow_attack;
        public float close_range;

        [SerializeField]
        public Controller.InputCode input_to_attack = Controller.InputCode.ATTACK;

        public override AI define() {
            if ( hover ) {
                return AI.root(self, controller)
                    .def(AI.Def.Pattern.InputHorizontalTowardPlayer(this))
                    .def(AI.Def.Pattern.Hover(this, 2.0f))
                    .def(AI.Def.Name("Check allow attack")
                         .StopIf(_ => !allow_attack))
                    .def(AI.Def.Name("Attack")
                         .If(s => self.distanceXTo(s.player) <= close_range)
                         .Then(_ => controller.inputKey(input_to_attack)));

            }
            return AI.root(self, controller)
                .def(AI.Def.Pattern.InputTowardPlayer(this))
                .def(AI.Def.Name("Check allow attack")
                     .StopIf(_ => !allow_attack))
                .def(AI.Def.Name("Attack")
                     .If(s => self.distanceXTo(s.player) <= close_range)
                     .Then(_ => controller.inputKey(input_to_attack)));
        }
    }
}
