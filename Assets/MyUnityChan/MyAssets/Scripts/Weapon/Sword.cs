using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class Sword : Weapon {
        public override void setAttackAction(Player player) {
            PlayerAttack attack_manager =
                player.action_manager.getAction<PlayerAttack>("ATTACK");

            specs.ForEach(spec => {
                PlayerSlashBase slash = null;
                PlayerAttackSlotBase slot = null;
                switch ( spec.slot ) {
                    case Const.ID.AttackSlotType.LIGHT:
                        slash = new PlayerSlashL(player); slot = attack_manager.light; break;
                    case Const.ID.AttackSlotType.MIDDLE:
                        slash = new PlayerSlashM(player); slot = attack_manager.middle; break;
                    case Const.ID.AttackSlotType.HEAVY:
                        slash = new PlayerSlashH(player); slot = attack_manager.heavy; break;
                    case Const.ID.AttackSlotType.UP:
                        slash = new PlayerSlashUp(player); slot = attack_manager.up; break;
                    default: break;
                }
                if ( slash != null && slot != null ) {
                    slash.spec = spec;
                    slot.switchTo(slash);
                }
            });
        }
    }
}