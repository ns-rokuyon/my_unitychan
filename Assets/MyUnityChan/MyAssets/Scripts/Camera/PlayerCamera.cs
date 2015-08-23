using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class PlayerCamera : ObjectBase {
        private GameObject player = null;
        private Player player_component = null;

        private float area_limit_offset = 1.0f;

        // Use this for initialization
        void Start() {
            GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update() {
            if ( player ) {
                Area area = AreaManager.Instance.getArea(player_component.getAreaName());
                Vector3 newpos = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z - 5.0f);
                if ( area ) {
                    if ( area.right_wall && newpos.x + area_limit_offset >= area.limitRight() ) {
                        newpos.x = transform.position.x;    // no update
                    }
                    if ( area.left_wall && newpos.x - area_limit_offset <= area.limitLeft() ) {
                        newpos.x = transform.position.x;    // no update
                    }
                    if ( area.roof && newpos.y + area_limit_offset >= area.limitRoof() ) {
                        newpos.y = transform.position.y;    // no update
                    }
                    if ( area.floor && newpos.y - area_limit_offset <= area.limitFloor() ) {
                        newpos.y = transform.position.y;    // no update
                    }
                }
                transform.position = newpos;
            }
        }

        public void setPlayer(GameObject target) {
            // set target gameobject
            player = target;
            player_component = player.GetComponent<Player>();
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z - 5.0f);
        }
    }
}
