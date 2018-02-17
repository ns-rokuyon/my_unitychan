using UnityEngine;
using System.Collections;


namespace MyUnityChan {
    [System.Serializable]
    public class PrefabConfig {
        [SerializeField]
        public GameObject prefab;
    }

    // Representation of one of prefab list row
    // KeyValuePair(key=Const.ID.*, value=PrefabConfig)
    [System.Serializable]
    public class PrefabListRow<I, C> : KV<I, C> {
        public PrefabListRow(I id, C config) : base(id, config) {
        }
    }
}
