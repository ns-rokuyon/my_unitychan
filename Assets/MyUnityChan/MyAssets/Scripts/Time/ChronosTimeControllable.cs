using UnityEngine;
using System.Collections;
using Chronos;

namespace MyUnityChan {
    [RequireComponent(typeof(Timeline))]
    public class ChronosTimeControllable : TimeControllable {
        private Timeline _timeline;
        public Timeline timeline {
            get {
                return _timeline ?? (_timeline = GetComponent<Timeline>());
            }
        }

        public override bool paused {
            get {
                return timeline.timeScale == 0.0f;
            }
        }

        public override float deltaTime {
            get {
                return timeline.deltaTime;
            }
        }
    }
}