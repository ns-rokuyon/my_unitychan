using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class EnemyAttack : EnemyActionBase {
        public RingBuffer<EnemyAttack.Record> history { get; protected set; }

        public EnemyAttack(Character character) : base(character) {
            history = new RingBuffer<Record>(5);
        }

        public override void perform() {
            if ( enemy is IEnemyAttack ) {
                int id = (enemy as IEnemyAttack).onAttack(history);
                if ( id >= 0 )
                    history.add(new Record(id, Time.frameCount));
            }
        }

        public override bool condition() {
            return controller.keyAttack() && !enemy.isFlinching() && !enemy.isFrozen();
        }

        public override string name() {
            return "ATTACK";
        }

        public bool isNotFrameAllowedToAttack() {
            if ( !history.isEmpty() && 
                Math.Abs(Time.frameCount - history.getHead().frame) <= Const.Frame.ENEMY_ATTACK_MIN_INTERVAL ) {
                return true;
            }
            return false;
        }

        public class Record {
            public int id { get; set; }     // Attack ID
            public int frame { get; set; }  // Last attack frame

            public Record(int _id, int _frame) {
                id = _id;
                frame = _frame;
            }
        }
    }
}
