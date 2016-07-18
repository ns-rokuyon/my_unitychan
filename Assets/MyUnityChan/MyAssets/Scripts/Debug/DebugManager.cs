using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class DebugManager : SingletonObjectBase<DebugManager> {
        [SerializeField]
        public Const.Loglevel loglevel;
        public bool lock_logging;

        // Use this for initialization
        void Start() {
            SettingManager.setCallback(Settings.Flag.GET_ALL_ABILITIES, flag => {
                if ( flag ) AbilityItem.setAllAbilitiesToPlayer();
            });
            SettingManager.setCallback(Settings.Flag.SHOW_HITBOX, flag => {
                Hitbox.RENDER_HITBOX = flag;
            });
        }

        public static void log(object message, Const.Loglevel level = Const.Loglevel.INFO) {
            if ( Instance.lock_logging ) return;
            if ( Instance.loglevel > level ) return;

            printLog(message, level);
        }

        public static void printLog(object message, Const.Loglevel level) {
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

        public void drawLine(Vector3 pa, Vector3 pb) {
            LineRenderer renderer = GetComponent<LineRenderer>();
            renderer.SetVertexCount(2);
            renderer.SetPosition(0, pa);
            renderer.SetPosition(1, pb);
        }

    }
}
