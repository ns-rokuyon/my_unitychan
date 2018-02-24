using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "DamageObjectConfigTable", menuName = "ConfigTable/DamageObjectConfigTable")]
    public class DamageObjectConfigTable : PrefabListBasedConfigTable<Const.ID.DamageObject, DamageObjectPrefabConfig, DamageObjectConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.DamageObject))]
        public List<Row> damage_objects;

        public override List<Row> prefab_list {
            get {
                return damage_objects;
            }
        }

        public override Dictionary<Const.ID.DamageObject, string> prefab_path_map {
            get {
                return Const.Prefab.DamageObject;
            }
        }

        public override Row createPrefabListRow(Const.ID.DamageObject id) {
            return new Row(id, new DamageObjectPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.DamageObject, DamageObjectPrefabConfig> {
            public Row(Const.ID.DamageObject id, DamageObjectPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class DamageObjectPrefabConfig : PrefabConfig {
    }
}