using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class SensorBomb : Bomb {
        [SerializeField]
        private SensorZone sensor_zone;

        public bool ready { get; private set; }

        public override void initialize() {
            ready = true;
            DebugManager.log("sensor bomb initialize!!!!");
            if ( sensor_zone )
                sensor_zone.onEnemyEnterCallback = onEnter;
        }

        public override void finalize() {
            ready = false;
            if ( sensor_zone )
                sensor_zone.onEnemyEnterCallback = null;
        }

        public void onPlayerEnter(Player player, Collider colliderInfo) {
            DebugManager.log("onPlayerEnter!!!!");
            explode();
            finalize();
        }

        public void onEnter(Enemy enemy, Collider colliderInfo) {
            DebugManager.log("onEnter!!!!");
            explode();
            finalize();
        }

        public override Vector3 getInitPosition(Transform owner) {
            return owner.position.add(owner.forward.x * 0.5f, 0.2f, 0);
        }
    }
}