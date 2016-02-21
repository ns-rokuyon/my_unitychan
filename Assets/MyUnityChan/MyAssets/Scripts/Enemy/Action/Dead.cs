using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyDead : EnemyActionBase {
        private string effect_path;
        public delegate void DeadCallbackFunc();
        private DeadCallbackFunc dead_callback;

        public override string name() {
            return "ENEMY_DEAD";
        }

        public EnemyDead(Character character, string dead_effect_path, DeadCallbackFunc dead_func)
            : base(character) {
                effect_path = dead_effect_path;
                dead_callback = dead_func;
        }

        public override void perform() {
            enemy.setHP(0);
            dead_callback();
            GameObject obj = ObjectPoolManager.getGameObject(effect_path);
            obj.setParent(Hierarchy.Layout.EFFECT).GetComponent<Effect>().ready(enemy.transform.position, 60, effect_path);
            enemy.destroyHpGauge();
            enemy.gameObject.SetActive(false);
        }

        public override bool condition() {
            return enemy.getHP() <= 0;
        }

    }

}