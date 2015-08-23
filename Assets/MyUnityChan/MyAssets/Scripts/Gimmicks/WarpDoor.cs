using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WarpDoor : Warp {

        // Use this for initialization
        void Start() {
            AreaManager.self().registerAreaConnectionInfo(this.gameObject, warp_to);
        }

        // Update is called once per frame
        void Update() {

        }

        public override bool condition(Player player) {
            float vertical = player.getController().keyVertical();
            return !player.isFrozen() && vertical > 0;
        }

        public override void warp(Player player) {
            // warp
            player.transform.position = warp_to.transform.position;
            player.lookAtDirectionX(warp_to.GetComponent<Warp>().dst_direction);
            player.freeze(false);
        }

    }
}
