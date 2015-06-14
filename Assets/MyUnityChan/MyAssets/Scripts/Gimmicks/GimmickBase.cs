using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MovingFloor : ObjectBase { 
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }
    }

    public abstract class Warp : ObjectBase {
        public GameObject warp_to;
        public float dst_direction;

        public abstract bool condition(Player player);
        public abstract void warp(Player player);

        public void OnTriggerStay(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                if ( condition(player) ) {
                    player.freeze();    // moving lock
                    CameraFade.StartAlphaFade(Color.black, false, 1f, 0f, () => {
                        warp(player);
                    });
                }
            }
        }
    }

    public abstract class Door : ObjectBase {
        public abstract void open();
        public abstract void close();
    }
}
