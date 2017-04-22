using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Zone : ObjectBase {
        virtual protected void onPlayerEnter(Player player) {
            Debug.Log("Enter:" + player.name);
        }

        virtual protected void onPlayerExit(Player player) {
            Debug.Log("Exit:" + player.name);
        }

        virtual protected void onEnemyEnter(Enemy enemy) {
            Debug.Log("Enter:" + enemy.name);
        }

        virtual protected void onEnemyExit(Enemy enemy) {
            Debug.Log("Exit:" + enemy.name);
        }

        public void OnTriggerEnter(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    onPlayerEnter(player);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    onEnemyEnter(enemy);
                    break;
            
                default:
                    break;
            }
        }

        public void OnTriggerExit(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    onPlayerExit(player);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    onEnemyExit(enemy);
                    break;
            
                default:
                    break;
            }
        }
    }
}