using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class DoubleJumpItem : AbilityItem {

        public override void perform(Player player) {
            player.getActionManager().registerAction(new PlayerDoubleJump(player));
            player.getActionManager().disableAction("JUMP");
        }

        public override void destroy(Player player) {
            ModalManager.Instance.show("DoubleJump", "You can jump in the air");
            PauseManager.Instance.pause(true, ModalManager.Instance.control, () => { ModalManager.Instance.hide(); });
            Destroy(this.gameObject);
        }
    }
}
