using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class TwoHandedSword : Weapon {
        public override void setAttackAction(Player player) {
            PlayerAttack attack_manager =
                player.action_manager.getAction<PlayerAttack>("ATTACK");

            specs.ForEach(spec => {
                PlayerSlashBase slash = null;
                PlayerAttackSlotBase slot = null;
                switch ( spec.slot ) {
                    case Const.ID.AttackSlotType.LIGHT:
                        slash = new PlayerTwoHandedSlashL(player); slot = attack_manager.light; break;
                    case Const.ID.AttackSlotType.MIDDLE:
                        slash = new PlayerTwoHandedSlashM(player); slot = attack_manager.middle; break;
                    case Const.ID.AttackSlotType.HEAVY:
                        slash = new PlayerTwoHandedSlashH(player); slot = attack_manager.heavy; break;
                    default: break;
                }
                if ( slash != null && slot != null ) {
                    slash.spec = spec;
                    slash.weapon = this;
                    slot.switchTo(slash);
                }
            });
        }
    }
}