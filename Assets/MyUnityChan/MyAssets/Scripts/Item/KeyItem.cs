using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class KeyItem : ObjectBase {
        [SerializeField]
        public Const.ID.SE se;

        void Awake() {
            setupSoundPlayer();
            awake();
        }

        public abstract void perform(Player player);

        public abstract void destroy(Player player);

        public virtual void awake() { }

        public void setPosition(Vector3 pos) {
            gameObject.transform.position = pos;
        }

        public void OnCollisionStay(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Player" ) {
                Player player = collisionInfo.gameObject.GetComponent<Player>();
                player.se(se);
                perform(player);
                destroy(player);
            }
        }

        public void OnTriggerStay(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                player.se(se);
                perform(player);
                destroy(player);
            }
        }
    }
}
