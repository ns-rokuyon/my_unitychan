using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnemyDead : EnemyActionBase {
        private string effect_path;

        public override string name() {
            return "ENEMY_DEAD";
        }

        public EnemyDead(Character character, string dead_effect_path)
            : base(character) {
                effect_path = dead_effect_path;
        }

        public override void perform() {
            enemy.setHP(0);
            GameObject obj = ObjectPoolManager.getGameObject(effect_path);
            obj.setParent(Hierarchy.Layout.EFFECT).GetComponent<Effect>().ready(enemy.transform.position, 60, effect_path);
            GameObject.Destroy(enemy.gameObject);
        }

        public override bool condition() {
            return enemy.getHP() <= 0;
        }

    }

}