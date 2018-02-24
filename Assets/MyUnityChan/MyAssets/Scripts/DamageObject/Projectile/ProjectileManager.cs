using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ProjectileManager : PrefabManagerBase<ProjectileManager> {
        public override string parent {
            get {
                return Hierarchy.Layout.PROJECTILE;
            }
        }

        public static T createBeam<T>(Const.ID.Projectile.Beam beam_id, bool use_objectpool = true) {
            return Instance.create<T>(ConfigTableManager.Beam.getPrefabConfig(beam_id).prefab,
                                      use_objectpool);
        }

        public static T createMissile<T>(Const.ID.Projectile.Missile missile_id, bool use_objectpool = true) {
            return Instance.create<T>(ConfigTableManager.Missile.getPrefabConfig(missile_id).prefab,
                                      use_objectpool);
        }
    }
}