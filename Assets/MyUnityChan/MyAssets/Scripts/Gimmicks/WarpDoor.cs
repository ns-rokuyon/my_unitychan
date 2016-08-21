using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WarpDoor : Warp {

        // Use this for initialization
        void Start() {
            AreaManager.self().registerAreaConnectionInfo(this.gameObject, warp_to);
        }

        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.lookAtDirectionX(warp_to.GetComponent<Warp>().dst_direction);
            player.freeze(false);
            player.getPlayerCamera().warpByPlayer(player);
        }

        public override void onPlayerInputUp(Player player) {
            player.freeze();    // moving lock
            CameraFade.StartAlphaFade(Color.black, false, 1f, 0f, () => {
                warp(player);
            });
        }
    }
}
