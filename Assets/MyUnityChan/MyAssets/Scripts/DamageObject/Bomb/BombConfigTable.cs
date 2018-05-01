using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "BombConfigTable", menuName = "ConfigTable/BombConfigTable")]
    public class BombConfigTable : PrefabListBasedConfigTable<Const.ID.Bomb, BombPrefabConfig, BombConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Bomb))]
        public List<Row> bombs;

        public override List<Row> prefab_list {
            get {
                return bombs;
            }
        }

        public override Dictionary<Const.ID.Bomb, string> prefab_path_map {
            get {
                return Const.Prefab.Bomb;
            }
        }

        public override Row createPrefabListRow(Const.ID.Bomb id) {
            return new Row(id, new BombPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Bomb, BombPrefabConfig> {
            public Row(Const.ID.Bomb id, BombPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class BombPrefabConfig : PrefabConfig {
    }
}
