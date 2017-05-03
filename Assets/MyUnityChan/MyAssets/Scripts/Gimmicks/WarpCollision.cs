using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class WarpCollision : Warp {
        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.lookAtDirectionX(warp_to.GetComponent<Warp>().dst_direction);
            player.freeze(false, 30);
            player.getPlayerCamera().onChangeArea();
        }

        public override void onPlayerEnter(Player player) {
            player.freeze();    // moving lock
            player.manager.camera.zoom(player.manager.camera.getZoomPointOnChangeArea(), 0.8f);
            player.getPlayerCamera().fadeOut(Const.Frame.AREA_TRANSITION_FADE);
            delay(Const.Frame.AREA_TRANSITION_FADE, () => {
                warp(player);
                player.getPlayerCamera().fadeIn(
                    Const.Frame.AREA_TRANSITION_FADE,
                    delay_frame:Const.Frame.AREA_TRANSITION_KEEP_BLACKOUT);
            });
        }
    }
}