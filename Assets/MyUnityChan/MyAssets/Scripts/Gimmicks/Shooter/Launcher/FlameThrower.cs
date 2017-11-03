using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class FlameThrower : LauncherBase {

        public override void Start() {
            base.Start();
            //setProjectile("FLAME_THROWER");
        }

        public override void shoot() {
            DebugManager.warn("FT.shoot()");
            GameObject obj = ObjectPoolManager.getGameObject(Const.Prefab.Projectile["FLAME_THROWER"]);
            obj.setParent(Hierarchy.Layout.PROJECTILE);

            Beam beam = obj.GetComponent<Beam>();
            beam.time_control.changeClock(owner.time_control.clockName);
            beam.setDir(direction);
            beam.setStartPosition(this.gameObject.transform.position + muzzle_offset);
            beam.linkShooter(this);

            obj.transform.LookAt(GameStateManager.getPlayer().transform);

            // sound
            sound();
        }
    }
}
