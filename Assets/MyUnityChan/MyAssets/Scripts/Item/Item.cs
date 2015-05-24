using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Item : PoolObjectBase {

        public abstract void perform(Player player);

        public void OnCollisionStay(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Player" ) {
                Player player = collisionInfo.gameObject.GetComponent<Player>();
                perform(player);
            }
        }
    }
}
