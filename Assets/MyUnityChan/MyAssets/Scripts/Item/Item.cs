using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Item : PoolObjectBase {

        public abstract void perform(Player player);

        public abstract void destroy(Player player);

        public void setPosition(Vector3 pos) {
            gameObject.transform.position = pos;
        }

        public void OnCollisionStay(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Player" ) {
                Player player = collisionInfo.gameObject.GetComponent<Player>();
                perform(player);
                destroy(player);
            }
        }

        public void OnTriggerStay(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                perform(player);
                destroy(player);
            }
        }
    }
}
