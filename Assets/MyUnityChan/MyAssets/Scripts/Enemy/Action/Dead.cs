using UnityEngine;
using System.Collections;
using System;
using UniRx;

namespace MyUnityChan {
    public class EnemyDead : EnemyActionBase {
        public IDisposable dying;

        public override string name() {
            return "ENEMY_DEAD";
        }

        public EnemyDead(Character character) : base(character) {
            dying = null;
        }

        public override void init() {
            if ( dying != null )
                dying.Dispose();
            dying = null;
        }

        public override void perform() {
            enemy.setHP(0);

            if ( enemy is ICharacterDying ) {
                ICharacterDying en = enemy as ICharacterDying;
                dying = Observable.FromCoroutine(en.onDying)
                    .Subscribe(_ => {
                        onDead();
                    })
                    .AddTo(enemy);
            }
            else {
                onDead();
            }
        }

        protected void onDead() {
            // OnDead behavior
            if ( enemy is IEnemyDead ) {
                IEnemyDead en = enemy as IEnemyDead;
                en.onDead();
                en.createDeadEffect();
            }

            // DropItem
            if ( enemy is IEnemyItemDrop ) {
                //Const.ID.Item itemname = (enemy as IEnemyItemDrop).dropItem();
                //DropItem item = DropItemManager.createItem<DropItem>(itemname, true);
                //item.setPosition(enemy.transform.position + Vector3.up);
                var dropper = enemy.GetComponent<ItemDropper>();
                if ( dropper ) {
                    dropper.drop();
                }
            }

            // Discard
            enemy.deactivate();
        }

        public override bool condition() {
            return enemy.getHP() <= 0 && dying == null;
        }

    }

}