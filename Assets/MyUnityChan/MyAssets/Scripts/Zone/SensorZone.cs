using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SensorZone : Zone {
        public bool only_firsttime;

        public System.Action<Player, Collider> onPlayerEnterCallback { get; set; }
        public System.Action<Player, Collider> onPlayerStayCallback { get; set; }
        public System.Action<Player, Collider> onPlayerExitCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyEnterCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyStayCallback { get; set; }
        public System.Action<Enemy, Collider> onEnemyExitCallback { get; set; }

        protected override void onPlayerEnter(Player player, Collider colliderInfo) {
            if ( onPlayerEnterCallback == null ) return;
            onPlayerEnterCallback(player, colliderInfo);
            if ( only_firsttime )
                gameObject.SetActive(false);
        }

        protected override void onPlayerStay(Player player, Collider colliderInfo) {
            if ( onPlayerStayCallback == null ) return;
            onPlayerStayCallback(player, colliderInfo);
        }

        protected override void onPlayerExit(Player player, Collider colliderInfo) {
            if ( onPlayerExitCallback == null ) return;
            onPlayerExitCallback(player, colliderInfo);
        }

        protected override void onEnemyEnter(Enemy enemy, Collider colliderInfo) {
            if ( onEnemyEnterCallback == null ) return;
            onEnemyEnterCallback(enemy, colliderInfo);
            if ( only_firsttime )
                gameObject.SetActive(false);
        }

        protected override void onEnemyStay(Enemy enemy, Collider colliderInfo) {
            if ( onEnemyStayCallback == null ) return;
            onEnemyStayCallback(enemy, colliderInfo);
        }

        protected override void onEnemyExit(Enemy enemy, Collider colliderInfo) {
            if ( onEnemyExitCallback == null ) return;
            onEnemyExitCallback(enemy, colliderInfo);
        }
    }
}