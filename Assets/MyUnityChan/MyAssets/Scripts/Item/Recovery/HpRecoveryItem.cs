﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class HpRecoveryItem : RecoveryItem {
        public int heal;

        public override void perform(Player player) {
            Debug.Log("healing");
            player.setHP(player.getHP() + heal);
        }

        public override void destroy(Player player) {
            if ( isPooledObject() ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, Const.Prefab.Item["HP_RECOVERY"]);
            }
            else {
                Destroy(this.gameObject);
            }
        }

        public override void initialize() {
        }

        public override void finalize() {
        }
    }
}