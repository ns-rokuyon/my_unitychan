using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

namespace MyUnityChan {
    public class DirectorZoneManagedPlayableBehaviour : PlayableBehaviour {
        public GameObject clip { get; set; }
        public DirectorZone director_zone { get; set; }

        public override void OnGraphStop(Playable playable) {
            DebugManager.warn("Graph Stop!");
            director_zone.onTimelineEnd(playable);
        }
    }
}
