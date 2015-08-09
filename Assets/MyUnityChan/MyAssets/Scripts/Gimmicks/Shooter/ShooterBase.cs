using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class ShooterBase : ObjectBase {
        public int shooting_frame;
        public int interval_frame;

        protected int start_frame;
        protected int frame_count;
        protected bool sleep;
        protected int sleep_start_frame;
    }
}
