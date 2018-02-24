using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "BeamConfigTable", menuName = "ConfigTable/BeamConfigTable")]
    public class BeamConfigTable : PrefabListBasedConfigTable<Const.ID.Projectile.Beam, BeamPrefabConfig, BeamConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Projectile.Beam))]
        public List<Row> beams;

        public override List<Row> prefab_list {
            get {
                return beams;
            }
        }

        public override Dictionary<Const.ID.Projectile.Beam, string> prefab_path_map {
            get {
                return Const.Prefab.Projectile.Beam;
            }
        }

        public override Row createPrefabListRow(Const.ID.Projectile.Beam id) {
            return new Row(id, new BeamPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Projectile.Beam, BeamPrefabConfig> {
            public Row(Const.ID.Projectile.Beam id, BeamPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class BeamPrefabConfig : PrefabConfig {
    }
}
