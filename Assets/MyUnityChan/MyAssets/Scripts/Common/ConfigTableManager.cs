using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public class ConfigTableManager : SingletonObjectBase<ConfigTableManager> {
        [SerializeField]
        public BeamConfigTable beam_config_table;

        [SerializeField]
        public DamageObjectConfigTable damage_object_config_table;

        [SerializeField]
        public EffectConfigTable effect_config_table;

        [SerializeField]
        public HitboxConfigTable hitbox_config_table;

        [SerializeField]
        public ItemConfigTable item_config_table;

        [SerializeField]
        public MissileConfigTable missile_config_table;

        [SerializeField]
        public SprayConfigTable spray_config_table;

        /* Props */
        public static BeamConfigTable Beam {
            get { return Instance.beam_config_table; }
        }

        public static DamageObjectConfigTable DamageObject {
            get { return Instance.damage_object_config_table; }
        }

        public static EffectConfigTable Effect {
            get { return Instance.effect_config_table; }
        }

        public static HitboxConfigTable Hitbox {
            get { return Instance.hitbox_config_table; }
        }

        public static ItemConfigTable Item {
            get { return Instance.item_config_table; }
        }

        public static MissileConfigTable Missile {
            get { return Instance.missile_config_table; }
        }

        public static SprayConfigTable Spray {
            get { return Instance.spray_config_table; }
        }
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

        public C getPrefabConfig(I id) {
            return prefab_list.Find(r => r.key.Equals(id)).value;
        }
    }

}