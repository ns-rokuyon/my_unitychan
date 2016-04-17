using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerGuard : PlayerAction {
        public bool guarding { get; private set; }

        public PlayerGuard(Character character)
            : base(character) {
            guarding = false;
            flag = null;
        }

        public override string name() {
            return "GUARD";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.GUARD;
        }

        public override bool condition() {
            return controller.keyGuard();
        }

        public override void perform() {
            if ( !guarding ) {
                guarding = true;
                player.getAnimator().SetTrigger("GuardStart");
            }
        }

        public override void off_perform() {
            if ( guarding ) {
                guarding = false;
                player.getAnimator().CrossFade("Locomotion", 0.6f);
            }
        }

    }
}
