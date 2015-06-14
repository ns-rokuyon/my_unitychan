using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AreaGateWarpCollision : WarpCollision {
        private Door door = null;

        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.freeze(false);

            // close door
            if ( door == null ) {
                // cache
                door = gameObject.transform.parent.gameObject.transform.FindChild("Door").GetComponent<Door>();
            }
            door.close();
        }
    }
}
