using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MyUnityChan {
    [CustomEditor(typeof(ConfigTableManager), true)]
    public class ConfigTableManagerEditor : Editor {
        static readonly string prefab_base_path = "Assets/MyAssets/Resources/";

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var manager = target as ConfigTableManager;

            if ( GUILayout.Button("Set beam prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.beam_config_table);
            }

            if ( GUILayout.Button("Set damage_object prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.damage_object_config_table);
            }

            if ( GUILayout.Button("Set effect prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.effect_config_table);
            }

            if ( GUILayout.Button("Set hitbox prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.hitbox_config_table);
            }

            if ( GUILayout.Button("Set item prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.item_config_table);
            }

            if ( GUILayout.Button("Set missile prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.missile_config_table);
            }

            if ( GUILayout.Button("Set spray prefab references automatically") ) {
                initPrefabListBasedConfigTable(manager.spray_config_table);
            }
        }

        // Set prefab references to PrefabListBasedConfigTable automatically based on Const.ID.* and Const.Prefab.*
        private void initPrefabListBasedConfigTable<I, C, R>(PrefabListBasedConfigTable<I, C, R> config_table)
            where I : struct
            where C : PrefabConfig, new()
            where R : PrefabListRow<I, C> {

            foreach ( var _id in Enum.GetValues(config_table.enum_type) ) {
                int i = (int)_id;
                I id = (I)Enum.ToObject(config_table.enum_type, i);     // Const.ID.*

                if ( config_table.prefab_list.Count <= i ) {
                    var row = config_table.createPrefabListRow(id);
                    config_table.prefab_list.Add(row);
                }

                if ( config_table.prefab_path_map.ContainsKey(id) ) {
                    string path = prefab_base_path + config_table.prefab_path_map[id] + ".prefab";
                    GameObject loaded = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if ( config_table.prefab_list[i].value.prefab == null ) {
                        config_table.prefab_list[i].value.prefab = loaded;
                        if ( loaded == null ) {
                            Debug.LogWarning("The prefab " + path + " was not found");
                        }
                        else {
                            Debug.Log("Set prefab by " + Enum.GetName(config_table.enum_type, id));
                        }
                    }
                    else {
                        Debug.Log("Config table has already the prefab corresponding to " +
                            Enum.GetName(config_table.enum_type, id));
                    }

                    if ( loaded ) {
                        var poolobjects = loaded.GetComponents<PoolObjectBase>();
                        poolobjects.ToList().ForEach(po => po.prefab = loaded);
                    }
                }
            }
            EditorUtility.SetDirty(config_table);
        }

    }
}