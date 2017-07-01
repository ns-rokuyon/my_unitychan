using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DeadZone : Zone {
        public int damage;

        protected override void onPlayerEnter(Player player, Collider colliderInfo) {
            player.damage(damage);
            player.comeback(player.last_entrypoint);
        }

        protected override void onEnemyEnter(Enemy enemy, Collider colliderInfo) {
            enemy.damage(enemy.getHP());
        }

        protected override void onPlayerExit(Player player, Collider collierInfo) {
        }

        protected override void onEnemyExit(Enemy enemy, Collider collierInfo) {
        }
    }
}
