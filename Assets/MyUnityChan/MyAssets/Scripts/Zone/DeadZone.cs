using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DeadZone : Zone {
        public int damage;

        protected override void affectPlayer(Player player) {
            player.damage(damage);
            player.comeback(player.last_entrypoint);
        }

        protected override void affectEnemy(Enemy enemy) {
            enemy.damage(enemy.getHP());
        }
    }
}
