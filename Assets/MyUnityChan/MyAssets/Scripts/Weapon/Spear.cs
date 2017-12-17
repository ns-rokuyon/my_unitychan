using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class Spear : Weapon {
        public override void setAttackAction(Player player) {
            PlayerAttack attack_manager =
                player.action_manager.getAction<PlayerAttack>("ATTACK");

            specs.ForEach(spec => {
                PlayerThrustBase thrust = null;
                PlayerAttackSlotBase slot = null;
                switch ( spec.slot ) {
                    case Const.ID.AttackSlotType.LIGHT:
                        thrust = new PlayerThrustL(player); slot = attack_manager.light; break;
                    case Const.ID.AttackSlotType.MIDDLE:
                        thrust = new PlayerThrustM(player); slot = attack_manager.middle; break;
                    case Const.ID.AttackSlotType.HEAVY:
                        thrust = new PlayerThrustH(player); slot = attack_manager.heavy; break;
                    default: break;
                }
                if ( thrust != null && slot != null ) {
                    thrust.spec = spec;
                    thrust.weapon = this;
                    slot.switchTo(thrust);
                }
            });
        }
    }
}