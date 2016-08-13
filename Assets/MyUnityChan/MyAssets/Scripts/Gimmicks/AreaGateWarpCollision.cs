using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AreaGateWarpCollision : WarpCollision {
        private Door door = null;
        private readonly Vector3 player_down_shift = new Vector3(0.0f, 1.3f, 0.0f);

        public AreaGate gate { get; set; }

        public override void warp(Player player) {
            // Warp
            player.transform.position = warp_to.transform.position - player_down_shift;
            player.freeze(false);
            player.getPlayerCamera().warpByPlayer(player);

            // Callback
            gate.onPass();

            // Close door
            if ( door == null ) {
                // Cache
                door = gameObject.transform.parent.gameObject.transform.FindChild("Door").GetComponent<Door>();
            }
            door.close();
        }
    }
}
