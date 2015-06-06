using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WarpDoor : Warp {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTriggerStay(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                float vertical = player.getController().keyVertical();
                if ( !player.isFrozen() && vertical > 0 ) {
                    player.freeze();    // moving lock
                    CameraFade.StartAlphaFade(Color.black, false, 1f, 0f, () => {
                        // warp
                        player.transform.position = warp_to.transform.position;
                        player.freeze(false);
                    });
                }
            }
        }
    }
}
