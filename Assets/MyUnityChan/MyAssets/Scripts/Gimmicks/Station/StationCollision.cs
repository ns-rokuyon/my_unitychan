using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class StationCollision : TriggerCollision {
        public StationBase station { get; set; }

        void Awake() {
            station = GetComponentInParent<StationBase>();
        }

        public override void onPlayerInputUp(Player player) {
            if ( station )
                station.perform(player);
            else
                DebugManager.log("No station component in parent object", Const.Loglevel.ERROR);
        }
    }
}
