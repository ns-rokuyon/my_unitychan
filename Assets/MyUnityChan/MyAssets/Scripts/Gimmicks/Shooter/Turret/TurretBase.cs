using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class TurretBase : ShooterBase {
        public Vector3 base_angle = new Vector3(1.0f, 0.0f, 0.0f);

        public override Vector3 angle() {
            if ( this.gameObject.transform.forward.x >= 0 ) return base_angle;
            return base_angle.flipX();
        }
    }
}
