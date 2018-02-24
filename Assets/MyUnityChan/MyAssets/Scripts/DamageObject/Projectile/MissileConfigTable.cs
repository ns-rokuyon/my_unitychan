using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "MissileConfigTable", menuName = "ConfigTable/MissileConfigTable")]
    public class MissileConfigTable : PrefabListBasedConfigTable<Const.ID.Projectile.Missile, MissilePrefabConfig, MissileConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Projectile.Missile))]
        public List<Row> missiles;

        public override List<Row> prefab_list {
            get {
                return missiles;
            }
        }

        public override Dictionary<Const.ID.Projectile.Missile, string> prefab_path_map {
            get {
                return Const.Prefab.Projectile.Missile;
            }
        }

        public override Row createPrefabListRow(Const.ID.Projectile.Missile id) {
            return new Row(id, new MissilePrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Projectile.Missile, MissilePrefabConfig> {
            public Row(Const.ID.Projectile.Missile id, MissilePrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class MissilePrefabConfig : PrefabConfig {
    }
}
