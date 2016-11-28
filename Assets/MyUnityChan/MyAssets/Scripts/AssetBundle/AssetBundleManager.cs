using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class AssetBundleManager : SingletonObjectBase<AssetBundleManager> {
        public Dictionary<Const.ID.AssetBundle, Dictionary<string, Object>> assets { get; set; }
        public bool now_loading { get; set; }

        void Awake() {
            assets = new Dictionary<Const.ID.AssetBundle, Dictionary<string, Object>>();
            
            // Load all AssetBundle
            StartCoroutine(loadAll());
        }

        public static Object get(Const.ID.AssetBundle id, string key) {
            if ( self().now_loading )
                return null;
            try {
                return self().assets[id][key];
            }
            catch ( KeyNotFoundException e ) {
                DebugManager.warn("AssetBundleManager returns null for id=" + id + ", key=" + key);
                return null;
            }
        }

        protected IEnumerator loadAll() {
            DebugManager.log("Start: Loading AssetBundle");
            now_loading = true;
            foreach ( Const.ID.AssetBundle id in System.Enum.GetValues(typeof(Const.ID.AssetBundle)) ) {
                var coroutine = StartCoroutine(loadAssetBundle(id));
                yield return coroutine;
            }
            now_loading = false;
            DebugManager.log("Done: Loading AssetBundle");
        }

        protected IEnumerator loadAssetBundle(Const.ID.AssetBundle id) {
            WWW bundlefile = new WWW("file://" + pathToAssetBundle(Const.AssetBundlePath[id]));
            yield return bundlefile;

            var creator = AssetBundle.LoadFromMemoryAsync(bundlefile.bytes);
            while ( !creator.isDone ) {
                yield return null;
            }

            AssetBundle assetbundle = creator.assetBundle;
            if ( !assetbundle ) {
                DebugManager.log("Failed to load AssetBundle(" + id + ")", Const.Loglevel.ERROR);
                yield break;
            }

            var loader = assetbundle.LoadAllAssetsAsync();
            yield return loader;

            foreach ( Object a in loader.allAssets ) {
                if ( !assets.ContainsKey(id) )
                    assets.Add(id, new Dictionary<string, Object>());
                assets[id].Add(a.name, a);
            }

            bundlefile.Dispose();
        }

        protected string pathToAssetBundle(string path) {
            return Application.streamingAssetsPath + "/" + path;
        }
    }
}