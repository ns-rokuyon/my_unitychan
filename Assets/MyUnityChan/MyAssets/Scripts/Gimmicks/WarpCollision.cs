﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WarpCollision : Warp {
        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.lookAtDirectionX(warp_to.GetComponent<Warp>().dst_direction);
            player.freeze(false, 30);
            player.getPlayerCamera().warpByPlayer(player);
        }

        public override void onPlayerEnter(Player player) {
            player.freeze();    // moving lock
            player.manager.camera.zoom(player.transform.position.add(
                0, 0, -Mathf.Abs(player.manager.camera.now_camera_position.position_diff.z / 2.0f)), 0.8f);
            CameraFade.StartAlphaFade(Color.black, false, 1f, 0f, () => {
                warp(player);
            });
        }
    }
}