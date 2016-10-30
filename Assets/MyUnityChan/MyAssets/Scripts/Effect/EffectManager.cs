using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EffectManager : PrefabManagerBase<EffectManager> {

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T effect = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.EFFECT).GetComponent<T>();
                (effect as EffectBase).enablePool(resource_path);
                return effect;
            }
            Debug.Log("create : " + resource_path);
            return instantiatePrefab(resource_path, Hierarchy.Layout.EFFECT).GetComponent<T>();
        }

        public static EffectManager self() {
            return Instance.GetComponent<EffectManager>();
        }

        // [Usage]
        // (EffectManager.Instance as EffectManager).createEffect(
        //      resource_path,   : path to prefab
        //      pos,             : effect position (XYZ)
        //      frame,           : effect lifetime (frame)
        //      use_objectpool   : if you use object pool for this effect object, set true
        // )
        public GameObject createEffect(string resource_path, Vector3 pos, int frame, bool use_objectpool = false) {
            Effect effect = create<Effect>(resource_path, use_objectpool);
            effect.ready(pos, frame, resource_path);
            return effect.gameObject;
        }

        public GameObject createEffect(Const.ID.Effect effect_name, Vector3 pos, int frame, bool use_objectpool = false) {
            if ( effect_name == Const.ID.Effect._NO_EFFECT )
                return null;
            return createEffect(Const.Prefab.Effect[effect_name], pos, frame, use_objectpool);
        }
    }
}
