using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DeadZone : Zone {
        public int damage;

        protected override void onPlayerEnter(Player player) {
            player.damage(damage);
            player.comeback(player.last_entrypoint);
        }

        protected override void onEnemyEnter(Enemy enemy) {
            enemy.damage(enemy.getHP());
        }

        protected override void onPlayerExit(Player player) {
        }

        protected override void onEnemyExit(Enemy enemy) {
        }
    }
}
