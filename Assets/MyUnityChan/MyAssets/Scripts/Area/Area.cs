using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class Area : ObjectBase {
        // set true to limit player moving in this area
        public bool roof;
        public bool floor;
        public bool left_wall;
        public bool right_wall;

        public GameObject[] spawnable_enemies;

        private Dictionary<string, bool> ins;
        private float x_harf;
        private float y_harf;
        private float z_harf;

        void Awake() {
            ins = new Dictionary<string, bool>();
            Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;
            x_harf = (float)(bounds.size.x / 2.0);
            y_harf = (float)(bounds.size.y / 2.0);
            z_harf = (float)(bounds.size.z / 2.0);
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

        public bool isIn(Vector3 pos) {
            Vector3 area_center = transform.position;
            if ( area_center.x - x_harf > pos.x || area_center.x + x_harf < pos.x ) {
                return false;
            }
            if ( area_center.y - y_harf > pos.y || area_center.y + y_harf < pos.y ) {
                return false;
            }
            if ( area_center.z - z_harf > pos.z || area_center.z + z_harf < pos.z ) {
                return false;
            }
            return true;
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

                for ( int i = 0; i < spawnable_enemies.Length; i++ ) {
                    if ( !spawnable_enemies[i].activeSelf ) {
                        spawnable_enemies[i].GetComponent<Enemy>().spawn();
                    }
                }
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
