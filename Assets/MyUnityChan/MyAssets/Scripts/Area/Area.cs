using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class Area : ObjectBase {
        // set true to limit player moving in this area
        public bool roof;
        public bool floor;
        public bool left_wall;
        public bool right_wall;

        private Dictionary<string, bool> ins;
        private float x_harf;
        private float y_harf;

        void Awake() {
            ins = new Dictionary<string, bool>();
            Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
            x_harf = (float)(bounds.size.x / 2.0);
            y_harf = (float)(bounds.size.y / 2.0);
        }

        public bool isIn(string name) {
            if ( !ins.ContainsKey(name) ) {
                return false;
            }
            return ins[name];
        }

        public bool isIn(Player player) {
            return isIn(player.gameObject.name);
        }

        public float limitLeft() {
            return gameObject.transform.position.x - x_harf;
        }

        public float limitRight() {
            return gameObject.transform.position.x + x_harf;
        }

        public float limitRoof() {
            return gameObject.transform.position.y + y_harf;
        }

        public float limitFloor() {
            return gameObject.transform.position.y - y_harf;
        }

        private void register(string name) {
            if ( !ins.ContainsKey(name) ) {
                ins[name] = false;
            }
        }

        public void OnTriggerEnter(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                player.setAreaName(this.gameObject.name);
                string name = player.gameObject.name;
                register(name);
                ins[name] = true;
            }
        }

        public void OnTriggerExit(Collider colliderInfo) {
            if ( colliderInfo.gameObject.tag == "Player" ) {
                Player player = colliderInfo.gameObject.GetComponent<Player>();
                string name = player.gameObject.name;
                ins[name] = false;
            }
        }
    }
}
