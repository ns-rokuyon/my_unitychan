using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

namespace MyUnityChan {
    public class DirectorZoneManagedPlayableBehaviour : PlayableBehaviour {
        public GameObject clip { get; set; }
        public DirectorZone director_zone { get; set; }

        public override void OnGraphStart(Playable playable) {
            TimelineManager.onTimelineStart(director_zone.director);
        }

        public override void OnGraphStop(Playable playable) {
            TimelineManager.onTimelineEnd();
            director_zone.onTimelineEnd(playable);
        }
    }
}
