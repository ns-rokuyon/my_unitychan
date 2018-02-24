using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace MyUnityChan {
    public class PlayerSwitchBeam : PlayerAction {
        /*
         * Deprecated
         */

        public PlayerSwitchBeam(Character character)
            : base(character) {
        }

        public override string name() {
            return "SWITCH_BEAM";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SWITCH_BEAM;
        }

        public override void perform() {
            Const.ID.Projectile.Beam bname;
            if ( player.beam_slot.Count > 1 ) {
                // Beam rotation
                bname = player.beam_slot[0];
                player.beam_slot.Remove(bname);
                player.beam_slot.Add(bname);
            }

            if ( player.beam_slot.Count == 0 ) {
                // Empty slot
                bname = Const.ID.Projectile.Beam._NO_SET;
            }
            else {
                // Head beam in slot
                bname = player.beam_slot[0];
            }
            BeamTurret turret = player.GetComponent<BeamTurret>();
            turret.switchBeam(bname);
        }

        public override bool condition() {
            return controller.keySwitchBeam();
        }

    }
}
