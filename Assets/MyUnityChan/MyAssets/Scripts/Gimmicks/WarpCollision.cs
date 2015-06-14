using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WarpCollision : Warp {

        public override bool condition(Player player) {
            return !player.isFrozen();
        }

        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.lookAtDirectionX(warp_to.GetComponent<Warp>().dst_direction);
            player.freeze(false);
        }
    }
}