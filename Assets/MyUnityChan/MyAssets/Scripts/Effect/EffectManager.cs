﻿using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class EffectManager : PrefabManagerBase<EffectManager> {

        public override T create<T>(string resource_path, bool use_objectpool=false) {
            if ( use_objectpool ) {
                T effect = ObjectPoolManager.getGameObject(resource_path).setParent(Hierarchy.Layout.EFFECT).GetComponent<T>();
                DebugManager.log("EM -> " + effect);
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
        public static GameObject createEffect(string resource_path, Vector3 pos, int frame, bool use_objectpool = false) {
            Effect effect = Instance.create<Effect>(resource_path, use_objectpool);
            effect.ready(pos, frame, resource_path);
            return effect.gameObject;
        }

        public static GameObject createEffect(Const.ID.Effect effect_name, Vector3 pos, int frame, bool use_objectpool = false) {
            if ( effect_name == Const.ID.Effect._NO_EFFECT )
                return null;
            return createEffect(Const.Prefab.Effect[effect_name], pos, frame, use_objectpool);
        }

        public static GameObject createEffect(Const.ID.Effect effect_name, Vector3 pos, Vector3 offset, int frame, bool use_objectpool = true) {
            if ( effect_name == Const.ID.Effect._NO_EFFECT )
                return null;
            var offsetPos = pos + offset;
            return createEffect(effect_name, offsetPos, frame, use_objectpool);
        }

        public static GameObject createEffect(Const.ID.Effect effect_name, Character ch,
                                              float frontOffsetX, float offsetY, int frame, bool use_objectpool = true) {
            var fw = ch.getFrontVector();
            var offset = new Vector3(fw.x * frontOffsetX, offsetY, 0.0f);
            return createEffect(effect_name, ch.transform.position, offset, frame, use_objectpool);
        }

        public static GameObject createEffect(Const.ID.Effect effect_name, GameObject track, int frame, bool use_objectpool = true) {
            var effect = createEffect(effect_name, track.transform.position, frame, use_objectpool);

            IObservable<long> o;
            if ( frame >= 0 ) {
                o = Observable.IntervalFrame(1).Take(frame);
            } else {
                o = Observable.EveryUpdate();
            }
            o.Subscribe(_ => {
                effect.transform.position = track.transform.position;
            }).AddTo(track);
            return effect;
        }

        public static GameObject createEffect(Const.ID.Effect effect_name, GameObject track,
                                              float frontOffsetX, float offsetY, int frame, bool use_objectpool = true) {
            var ch = track.GetComponent<Character>();
            var effect = createEffect(effect_name, ch, frontOffsetX, offsetY, frame, use_objectpool);

            IObservable<long> o;
            if ( frame >= 0 ) {
                o = Observable.IntervalFrame(1).Take(frame);
            } else {
                o = Observable.EveryUpdate();
            }
            o.Subscribe(_ => {
                var offset = new Vector3();
                if ( ch ) {
                    var fw = ch.getFrontVector();
                    offset = new Vector3(fw.x * frontOffsetX, offsetY, 0.0f);
                }
                else {
                    offset = new Vector3(frontOffsetX, offsetY, 0.0f);
                }
                effect.transform.position = track.transform.position + offset;
            }).AddTo(track);
            return effect;
        }


        // Create TextEffect
        // =============================================
        public static GameObject createTextEffect(string text, string resource_path, Vector3 pos, int frame, bool use_objectpool = false) {
            GameObject o = createEffect(resource_path, pos, frame, use_objectpool);
            o.GetComponent<TextEffect>().text = text;
            return o;
        }

        public static GameObject createTextEffect(string text, Const.ID.Effect effect_name, Vector3 pos, int frame, bool use_objectpool = false) {
            GameObject o = createEffect(Const.Prefab.Effect[effect_name], pos, frame, use_objectpool);
            if ( !o )
                return null;
            o.GetComponent<TextEffect>().text = text;
            return o;
        }

        public static GameObject createTextEffect(string text, Const.ID.Effect effect_name, Vector3 pos, Vector3 offset, int frame, bool use_objectpool = true) {
            GameObject o = createEffect(effect_name, offset, frame, use_objectpool);
            if ( !o )
                return null;
            o.GetComponent<TextEffect>().text = text;
            return o;
        }

        public static GameObject createTextEffect(string text, Const.ID.Effect effect_name, Character ch,
                                              float frontOffsetX, float offsetY, int frame, bool use_objectpool = true) {
            GameObject o = createEffect(effect_name, ch, frontOffsetX, offsetY, frame, use_objectpool);
            if ( !o )
                return null;
            o.GetComponent<TextEffect>().text = text;
            return o;
        }

    }
}
