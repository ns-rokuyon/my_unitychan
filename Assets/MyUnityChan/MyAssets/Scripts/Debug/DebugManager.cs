using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class DebugManager : SingletonObjectBase<DebugManager> {
        [SerializeField]
        public Const.Loglevel loglevel;

        // Use this for initialization
        void Start() {
            SettingManager.setCallback(Settings.Flag.GET_ALL_ABILITIES, flag => {
                if ( flag ) AbilityItem.setAllAbilitiesToPlayer();
            });
        }

        public static void log(object message, Const.Loglevel level = Const.Loglevel.INFO) {
            if ( Instance.loglevel > level ) return;

            switch ( level ) {
                case Const.Loglevel.WARN:
                    Debug.LogWarning(message);
                    break;
                case Const.Loglevel.ERROR:
                case Const.Loglevel.FATAL:
                    Debug.LogError(message);
                    break;
                case Const.Loglevel.DEBUG:
                case Const.Loglevel.INFO:
                default:
                    Debug.Log(message);
                    break;
            }
        }
    }
}
