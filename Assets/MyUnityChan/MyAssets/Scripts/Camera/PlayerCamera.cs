using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        private GameObject player = null;

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            if ( player ) {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z - 4.0f);
            }
        }

        public void setPlayer(GameObject target) {
            // set target gameobject
            player = target;
        }
    }
}
