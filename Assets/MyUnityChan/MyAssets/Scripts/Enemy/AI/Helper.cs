using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract partial class AIModel : ObjectBase {
        /* Helper functions */

        public void inputTowardPlayer(Player player) {
            inputHorizontalTowardPlayer(player);
            inputVerticalTowardPlayer(player);
        }

        public void inputHorizontalTowardPlayer(Player player) {
            float target_x = player.getPrevPosition(6).x;
            float self_x = self.transform.position.x;

            if ( target_x < self_x )
                controller.inputHorizontal(-1.0f, 10);
            else
                controller.inputHorizontal(1.0f, 10);
        }

        public void inputVerticalTowardPlayer(Player player) {
            float target_y = player.getPrevPosition(6).y;
            float self_y = self.transform.position.y;

            if ( target_y < self_y )
                controller.inputVertical(-1.0f, 10);
            else
                controller.inputVertical(1.0f, 10);
        }

    }
}