using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

namespace MyUnityChan {
    public class StayAndJumpAI : AIModel {
        public float close_range;

        public override AI build() {
            return AI.root(self, controller)
                .def(AI.Def.Name("Input jump")
                     .If(state => Mathf.Abs(state.player.transform.position.x - self.transform.position.x) < close_range)
                     .Then(_ => controller.inputKey(Controller.InputCode.JUMP, 1)))
                .def(AI.Def.Name("Flip")
                     .Keep(_ => self.faceToPlayer()))
                .build();
        }
    }
}