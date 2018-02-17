using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class ConfigTableManager : SingletonObjectBase<ConfigTableManager> {
        [SerializeField]
        public EffectConfigTable effect_config_table;
    }

    public class ConfigTable : ScriptableObject {
    }

    public abstract class PrefabListBasedConfigTable<I, C, R> : ConfigTable
        where I : struct
        where C : PrefabConfig
        where R : PrefabListRow<I, C> {

        public abstract List<R> prefab_list {
            get;
        }

        public abstract Dictionary<I, string> prefab_path_map {
            get;
        }

        public Type enum_type {
            get {
                return typeof(I);
            }
        }

        public abstract R createPrefabListRow(I id);

    }

}