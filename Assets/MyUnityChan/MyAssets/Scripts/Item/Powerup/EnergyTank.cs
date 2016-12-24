using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class EnergyTank : PowerupItem {
        private static GameText name = GameText.text("Energy tank");
        private static GameText description = GameText.text("Max HP +100");

        public static int count { get; set; }

        public override void awake() {
            EnergyTank.count++;
        }

        public override void destroy(Player player) {
            ModalManager.Instance.show(name, description);
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }

        public override void perform(Player player) {
            player.manager.status.addEnergyTank();
            player.onGetPowerupItem();
        }

    }

}
