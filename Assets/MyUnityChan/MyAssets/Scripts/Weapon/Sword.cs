using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class Sword : Weapon {
        public override void setAttackAction(Player player) {
            PlayerAttack attack_manager =
                player.action_manager.getAction<PlayerAttack>("ATTACK");

            PlayerSlashL slashL = new PlayerSlashL(player);
            slashL.spec = getSpec(Const.ID.AttackLevel.LIGHT);
            attack_manager.light.switchTo(slashL);

            PlayerSlashM slashM = new PlayerSlashM(player);
            slashM.spec = getSpec(Const.ID.AttackLevel.MIDDLE);
            attack_manager.middle.switchTo(slashM);

            PlayerSlashH slashH = new PlayerSlashH(player);
            slashH.spec = getSpec(Const.ID.AttackLevel.HEAVY);
            attack_manager.heavy.switchTo(slashH);
        }
    }
}