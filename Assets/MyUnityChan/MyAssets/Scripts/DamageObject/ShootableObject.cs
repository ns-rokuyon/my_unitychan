using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract class ShootableObject : DamageObjectBase {
        public ShooterBase shooter { get; protected set; }

        public void linkShooter(ShooterBase sh) {
            shooter = sh;
        }

    }
}