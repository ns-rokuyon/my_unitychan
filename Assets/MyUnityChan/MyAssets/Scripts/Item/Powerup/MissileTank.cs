﻿using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class MissileTank : PowerupItem {
        private static GameText name = GameText.text("Missile tank");
        private static GameText description = GameText.text("missile max +5");

        public static int count { get; set; }

        public override void awake() {
            MissileTank.count++;
        }

        public override void destroy(Player player) {
            ModalManager.Instance.show(name, description);
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }

        public override void perform(Player player) {
            player.manager.status.addMissileTank();
            player.onGetPowerupItem();
        }
    }

}
