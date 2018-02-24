using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    [CreateAssetMenu(fileName = "HitboxConfigTable", menuName = "ConfigTable/HitboxConfigTable")]
    public class HitboxConfigTable : PrefabListBasedConfigTable<Const.ID.Hitbox, HitboxPrefabConfig, HitboxConfigTable.Row> {
        [SerializeField, EnumLabel(typeof(Const.ID.Hitbox))]
        public List<Row> hitboxes;

        public override List<Row> prefab_list {
            get {
                return hitboxes;
            }
        }

        public override Dictionary<Const.ID.Hitbox, string> prefab_path_map {
            get {
                return Const.Prefab.Hitbox;
            }
        }

        public override Row createPrefabListRow(Const.ID.Hitbox id) {
            return new Row(id, new HitboxPrefabConfig());
        }

        [System.Serializable]
        public class Row : PrefabListRow<Const.ID.Hitbox, HitboxPrefabConfig> {
            public Row(Const.ID.Hitbox id, HitboxPrefabConfig config) : base(id, config) {
            }
        }
    }

    [System.Serializable]
    public class HitboxPrefabConfig : PrefabConfig {
    }
}
