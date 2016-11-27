using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public interface IEnemyAttack {
        void attack();
    }

    public interface IEnemyDead {
        void onDead();
        void createDeadEffect();
    }

    public interface IEnemyWalk {
        void onForward();
        void onStay();
    }

    public interface IEnemyItemDrop {
        Const.ID.Item dropItem();
    }

    public interface IEnemyLevelUp {
        int getMaxExp();
        void levelUp();
    }
    
    public interface IEnemyTakeDamage {
        void takeDamage(int damage);
    }
}
