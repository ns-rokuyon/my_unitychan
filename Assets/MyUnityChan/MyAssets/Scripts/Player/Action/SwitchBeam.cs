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
            DebugManager.warn("Deprecated!!! Use QuickBeamSelector instead");
        }

        public override bool condition() {
            return controller.keySwitchBeam();
        }
    }
}
