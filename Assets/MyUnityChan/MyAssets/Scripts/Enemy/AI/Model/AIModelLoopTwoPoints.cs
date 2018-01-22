using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    [RequireComponent(typeof(TwoPointsPath))]
    public class AIModelLoopTwoPoints : AIModel {
        protected TwoPointsPath path;
        public float direction_x;

        public enum Routines {
            BASE
        }

        public override void init() {
            path = GetComponent<TwoPointsPath>();
            direction_x = self.isLookAhead() ? 1.0f : -1.0f;
        }

        public override void define() {
            AI ai = AI.root(self, controller)
                .def(AI.Def.Name("No stay at single point")
                     .If(_ => self.getPositionHistoryCount() >= 10 && self.getRecentTravelDistance().x < 0.05f)
                     .Then(_ => {
                         direction_x *= -1;
                         controller.inputHorizontal(direction_x);
                         self.clearPositionHistory();
                     })
                     .Break())
                .def(AI.Def.Name("Turn to left")
                     .If(_ => direction_x > 0 && self.transform.position.x > path.right_point.x)
                     .Then(_ => {
                         direction_x = -1;
                         controller.inputHorizontal(direction_x);
                     })
                     .Break())
                .def(AI.Def.Name("Turn to right")
                     .If(_ => direction_x < 0 && self.transform.position.x < path.left_point.x)
                     .Then(_ => {
                         direction_x = 1;
                         controller.inputHorizontal(direction_x);
                     })
                     .Break())
                .def(AI.Def.Name("Go forward")
                     .Keep(_ => controller.inputHorizontal(direction_x)));

            registerRoutine(Routines.BASE, ai);
        }
    }
}
