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

    public class EnemySpecialAttack : EnemyAttack {
        public EnemySpecialAttack(Character character) : base(character) {
        }

        public override string name() {
            return "SPECIAL_ATTACK";
        }

        public override void perform() {
            if ( !(enemy is IEnemySpecialAttack ) )
                return;
            if ( controller.keySpecial01() )
                (enemy as IEnemySpecialAttack).onSpecialAttack01(history);
            else if ( controller.keySpecial02() )
                (enemy as IEnemySpecialAttack).onSpecialAttack02(history);
            else if ( controller.keySpecial03() )
                (enemy as IEnemySpecialAttack).onSpecialAttack03(history);
            else if ( controller.keySpecial04() )
                (enemy as IEnemySpecialAttack).onSpecialAttack04(history);
            else if ( controller.keySpecial05() )
                (enemy as IEnemySpecialAttack).onSpecialAttack05(history);
        }

        public override bool condition() {
            bool keyed = controller.keySpecial01() || controller.keySpecial02() || controller.keySpecial03() ||
                         controller.keySpecial04() || controller.keySpecial05();
            return keyed && !enemy.isFlinching() && !enemy.isFrozen();
        }
    }
}
