using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerBomb : PlayerAction {

        public PlayerBomb(Character character)
            : base(character) {
        }

        public override string name() {
            return "BOMB";
        }

        public override void perform() {
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.DamageObject["BOMB"]);
            obj.setParent(Hierarchy.Layout.DAMAGE_OBJECT);
            obj.GetComponent<TimeBomb>().setStartPosition(player.transform.position);

            player.lockInput(10);
        }

        public override bool condition() {
            return controller.keyAttack();
        }
    }

}
