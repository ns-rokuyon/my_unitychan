using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "ItemConfigTable", menuName = "ConfigTable/ItemConfigTable")]
    public class ItemConfigTable : PrefabListBasedConfigTable<Const.ID.Item, ItemPrefabConfig, ItemConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Item))]
        public List<Row> items;

        public override List<Row> prefab_list {
            get {
                return items;
            }
        }

        public override Dictionary<Const.ID.Item, string> prefab_path_map {
            get {
                return Const.Prefab.Item;
            }
        }

        public override Row createPrefabListRow(Const.ID.Item id) {
            return new Row(id, new ItemPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Item, ItemPrefabConfig> {
            public Row(Const.ID.Item id, ItemPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class ItemPrefabConfig : PrefabConfig {
    }
}
