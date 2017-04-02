using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class DebugManager : SingletonObjectBase<DebugManager> {
        [SerializeField]
        public Const.Loglevel loglevel;
        public bool lock_logging;
        public int print_freq = 1;

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
            if ( Instance.print_freq <= 0 ) return;
            if ( Time.frameCount % Instance.print_freq != 0 ) return;

            printLog(message, level);
        }

        public static void log(object message, GameObject obj, Const.Loglevel level = Const.Loglevel.INFO) {
            log("[" + obj.name + "] " + message, level);
        }

        public static void warn(object message, GameObject obj = null) {
            if ( obj )
                log(message, obj, Const.Loglevel.WARN);
            else
                log(message, Const.Loglevel.WARN);
        }

        public static void error(object message, GameObject obj = null) {
            if ( obj )
                log(message, obj, Const.Loglevel.ERROR);
            else
                log(message, Const.Loglevel.ERROR);
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

        private readonly int idle_state_id = Animator.StringToHash("Base Layer.Idle");
        private readonly int locomotion_state_id = Animator.StringToHash("Base Layer.Locomotion");
        private readonly int damage_state_id = Animator.StringToHash("Base Layer.Damage");

        public void printCurrentAnimationStateName(Animator animator) {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            var current = info.fullPathHash;

            if ( current == idle_state_id )
                log("CurrentAnimationStateName = Idle");
            else if ( current == locomotion_state_id )
                log("CurrentAnimationStateName = Locomotion");
            else if ( current == damage_state_id )
                log("CurrentAnimationStateName = Damage");
            else
                warn("CurrentAnimationStateName = OTHERS");
        }

    }
}
