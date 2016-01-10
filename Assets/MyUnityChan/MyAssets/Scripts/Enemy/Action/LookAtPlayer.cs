using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyLookAtPlayer : EnemyActionBase {
        public override bool condition() {
            return true;
        }

        public override string name() {
            return "LOOK_AT_PLAYER";
        }

        public EnemyLookAtPlayer(Character character)
            : base(character) {
        }

        public override void perform() {
            enemy.lookAtDirectionX(
                Mathf.Sign(GameStateManager.getPlayerObject().transform.position.x - enemy.gameObject.transform.position.x));
        }
    }
}
