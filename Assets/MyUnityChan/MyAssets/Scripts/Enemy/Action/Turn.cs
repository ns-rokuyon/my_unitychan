using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyTurn : EnemyActionBase {
        public EnemyTurn(Character ch) : base(ch) {
        }

        public float vx {
            get {
                if ( enemy.action_manager.hasAction("KINEMATICS") ) {
                    return enemy.action_manager.getAction<EnemyKinematics>("KINEMATICS").vx;
                }
                return enemy.getVx();
            }
        }

        public override void perform() {
            float dir_x = controller.keyHorizontal();
            if ( dir_x > 0 && vx > 0 ) {
                enemy.lookAhead();
            }
            else if ( dir_x < 0 && vx < 0 ) {
                enemy.lookBack();
            }
        }

        public override bool condition() {
            return !enemy.isFlinching() && controller.keyHorizontal() != 0.0f;
        }

        public override string name() {
            return "TURN";
        }
    }
}