using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "EffectConfigTable", menuName = "ConfigTable/EffectConfigTable")]
    public class EffectConfigTable : PrefabListBasedConfigTable<Const.ID.Effect, EffectPrefabConfig, EffectConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Effect))]
        public List<Row> effects;

        public override List<Row> prefab_list {
            get {
                return effects;
            }
        }

        public override Dictionary<Const.ID.Effect, string> prefab_path_map {
            get {
                return Const.Prefab.Effect;
            }
        }

        public override Row createPrefabListRow(Const.ID.Effect id) {
            return new Row(id, new EffectPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Effect, EffectPrefabConfig> {
            public Row(Const.ID.Effect id, EffectPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class EffectPrefabConfig : PrefabConfig {
    }
}
