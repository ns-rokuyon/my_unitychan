using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    /* ApproachingPlayerAI

        Generic enemy AI
    */
    public class ApproachingPlayerAI : AIModel {
        public bool hover;
        public bool allow_attack;
        public bool allow_aim;
        public float close_range;
        public int attack_interval;

        [SerializeField]
        public Controller.InputCode input_to_attack = Controller.InputCode.ATTACK;

        public override AI define() {
            // Keep horizontal input
            AI ai = AI.root(self, controller).def(AI.Def.Pattern.InputHorizontalTowardPlayer(this));

            // Hover type
            if ( hover ) {
                // TODO: Parametarize delta_ground
                ai = ai.def(AI.Def.Pattern.Hover(this, 2.0f));
            }

            // Attack
            ai = ai.def(AI.Def.Name("CheckAllowAttack").StopIf(_ => GameStateManager.gameover || !allow_attack))
                   .def(AI.Def.Name("Attack")
                        .Interval(attack_interval)
                        .If(s => self.distanceXTo(s.player) <= close_range)
                        .Then(_ => controller.inputKey(input_to_attack)));

            // Keep aiming
            if ( allow_aim ) {
                ShooterBase shooter = GetComponent<ShooterBase>();
                if ( shooter ) {
                    ai = ai.def(AI.Def.Name("KeepAimForward").Keep(_ => shooter.aimForward()));
                }
            }

            return ai;
        }
    }
}
