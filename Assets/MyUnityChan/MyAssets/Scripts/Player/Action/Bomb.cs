using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class PlayerBomb : PlayerAction {

        public int stock { get; private set; }
        public int stock_max { get; private set; }
        public int reload_frame { get; private set; }

        public PlayerBomb(Character character)
            : base(character) {
            stock_max = 3;
            stock = stock_max;
            reload_frame = 180;

            Observable.IntervalFrame(reload_frame)
                .Subscribe(_ => {
                    if ( stock < stock_max ) stock++;
                }).AddTo(player.gameObject);
        }

        public override string name() {
            return "BOMB";
        }

        public override void perform() {
            if ( stock > 0 ) {
                GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.DamageObject["BOMB"]);
                obj.setParent(Hierarchy.Layout.DAMAGE_OBJECT);
                obj.GetComponent<TimeBomb>().setStartPosition(player.transform.position);

                stock--;

                player.lockInput(10);
            }
        }

        public override bool condition() {
            return controller.keyAttack();
        }

    }

}
