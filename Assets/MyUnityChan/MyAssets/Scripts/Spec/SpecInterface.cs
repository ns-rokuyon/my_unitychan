using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public interface IAttackSpec {
        void attack(Character character, Hitbox hitbox);
    }

    public interface IForceSpec {
        void force(Rigidbody rb, Hitbox hitbox);
    }

    public interface ISoundSpec {
        void playSound(ObjectBase o, Hitbox hitbox);
    }

    public interface IEffectSpec {
        void playEffect(ObjectBase o, Hitbox hitbox);
    }
}