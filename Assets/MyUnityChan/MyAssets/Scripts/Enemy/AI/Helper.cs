using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract partial class AIModel : ObjectBase {
        /* Helper functions */

        public void inputHorizontalTowardPlayer(Player player) {
            float target_x = player.getPrevPosition(6).x;
            float self_x = self.transform.position.x;

            if ( target_x < self_x )
                controller.inputHorizontal(-1.0f, 10);
            else
                controller.inputHorizontal(1.0f, 10);
        }
    }
}