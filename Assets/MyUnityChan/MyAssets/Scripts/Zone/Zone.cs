using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Zone : ObjectBase {

        virtual protected void affectPlayer(Player player) {
            Debug.Log("Enter:" + player.name);
        }

        virtual protected void affectEnemy(Enemy enemy) {
            Debug.Log("Enter:" + enemy.name);
        }

        public void OnTriggerEnter(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    affectPlayer(player);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    affectEnemy(enemy);
                    break;
            
                default:
                    break;
            }
        }
    }
}