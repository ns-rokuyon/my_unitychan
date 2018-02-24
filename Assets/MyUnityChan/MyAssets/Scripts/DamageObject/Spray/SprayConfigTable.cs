using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "SprayConfigTable", menuName = "ConfigTable/SprayConfigTable")]
    public class SprayConfigTable : PrefabListBasedConfigTable<Const.ID.Spray, SprayPrefabConfig, SprayConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Spray))]
        public List<Row> sprays;

        public override List<Row> prefab_list {
            get {
                return sprays;
            }
        }

        public override Dictionary<Const.ID.Spray, string> prefab_path_map {
            get {
                return Const.Prefab.Spray;
            }
        }

        public override Row createPrefabListRow(Const.ID.Spray id) {
            return new Row(id, new SprayPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Spray, SprayPrefabConfig> {
            public Row(Const.ID.Spray id, SprayPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class SprayPrefabConfig : PrefabConfig {
    }

}