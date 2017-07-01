using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Zone : ObjectBase {
        virtual protected void onPlayerEnter(Player player, Collider colliderInfo) {
        }

        virtual protected void onPlayerStay(Player player, Collider colliderInfo) {
        }

        virtual protected void onPlayerExit(Player player, Collider colliderInfo) {
        }

        virtual protected void onEnemyEnter(Enemy enemy, Collider colliderInfo) {
        }

        virtual protected void onEnemyStay(Enemy enemy, Collider colliderInfo) {
        }

        virtual protected void onEnemyExit(Enemy enemy, Collider colliderInfo) {
        }


        public void OnTriggerEnter(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    onPlayerEnter(player, colliderInfo);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    onEnemyEnter(enemy, colliderInfo);
                    break;
            
                default:
                    break;
            }
        }

        public void OnTriggerStay(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    onPlayerStay(player, colliderInfo);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    onEnemyStay(enemy, colliderInfo);
                    break;
            
                default:
                    break;
            }
        }

        public void OnTriggerExit(Collider colliderInfo) {
            switch ( colliderInfo.gameObject.tag ) {
                case "Player":
                    Player player = colliderInfo.gameObject.GetComponent<Player>();
                    onPlayerExit(player, colliderInfo);
                    break;

                case "Enemy":
                    Enemy enemy = colliderInfo.gameObject.GetComponent<Enemy>();
                    onEnemyExit(enemy, colliderInfo);
                    break;
            
                default:
                    break;
            }
        }
    }
}