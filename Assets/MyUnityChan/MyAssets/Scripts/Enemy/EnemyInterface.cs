using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public interface IEnemyAttack {
        int onAttack(RingBuffer<EnemyAttack.Record> history);
    }

    public interface IEnemySpecialAttack {
        void onSpecialAttack01(RingBuffer<EnemyAttack.Record> history);
        void onSpecialAttack02(RingBuffer<EnemyAttack.Record> history);
        void onSpecialAttack03(RingBuffer<EnemyAttack.Record> history);
        void onSpecialAttack04(RingBuffer<EnemyAttack.Record> history);
        void onSpecialAttack05(RingBuffer<EnemyAttack.Record> history);
    }

    public interface IEnemyDead {
        void onDead();
        void createDeadEffect();
    }

    public interface IEnemyItemDrop {
        Const.ID.Item dropItem();
    }

    public interface IEnemyLevelUp {
        int getMaxExp();
        void onLevelUp();
    }
    
    public interface IEnemyTakeDamage {
        void takeDamage(int damage);
    }

    public interface IEnemyTakeProjectile {
        void onTakeProjectile(ProjectileSpec spec);
    }

    public interface IEnemyKnockback {
        void onKnockback();
        int getKnockbackThreshold();
    }

    public interface IEnemyStun {
        void onStun();
    }

}
