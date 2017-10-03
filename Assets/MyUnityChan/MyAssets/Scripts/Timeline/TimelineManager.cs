using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class TimelineManager : SingletonObjectBase<TimelineManager> {
        [ReadOnly] public bool is_playing;

        public PlayableDirector director { get; set; }

        public static double timeline_time {
            get {
                if ( isPlaying )
                    return Instance.director.time;
                return -1.0f;
            }
        }

        void Start() {
            this.UpdateAsObservable()
                .Where(_ => timeline_time >= Const.Sec.ALLOW_SKIP_TIMELINE_ELAPSED_TIME)
                .Where(_ => GameStateManager.getPlayer().manager.controller.getRawInput(Controller.InputCode.JUMP))
                .Subscribe(_ => {
                    skipTimeline();
                })
                .AddTo(this);
        }

        public static bool isPlaying {
            get {
                return Instance.is_playing;
            }
            set {
                Instance.is_playing= value;
            }
        }

        public static void onTimelineStart(PlayableDirector current_director) {
            Instance.director = current_director;
            isPlaying = true;
        }

        public static void onTimelineEnd() {
            isPlaying = false;
        }

        public static void skipTimeline() {
            Instance.director.time = Instance.director.duration - 0.01f;
        }
    }
}