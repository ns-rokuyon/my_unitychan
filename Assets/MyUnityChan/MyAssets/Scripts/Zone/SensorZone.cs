using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class SensorZone : Zone {
        public System.Action onPlayerEnterCallback { get; set; }
        public System.Action onPlayerExitCallback { get; set; }
        public System.Action onEnemyEnterCallback { get; set; }
        public System.Action onEnemyExitCallback { get; set; }

        protected override void onPlayerEnter(Player player) {
            if ( onPlayerEnterCallback == null ) return;
            onPlayerEnterCallback();
        }

        protected override void onPlayerExit(Player player) {
            if ( onPlayerExitCallback == null ) return;
            onPlayerExitCallback();
        }

        protected override void onEnemyEnter(Enemy enemy) {
            if ( onEnemyEnterCallback == null ) return;
            onEnemyEnterCallback();
        }

        protected override void onEnemyExit(Enemy enemy) {
            if ( onEnemyExitCallback == null ) return;
            onEnemyExitCallback();
        }
    }
}