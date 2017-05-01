using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    // Light attacks
    // =============================================================
    public class PlayerSlashL : PlayerAction {
        public AttackSpec spec { get; set; }
        protected int total_frame = 20;

        public PlayerSlashL(Character character)
            : base(character) {
            spec = null;
            use_transaction = true;
        }

        public override string name() {
            return "SLASH_L";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction._UNCLASSIFIED;
        }

        public override void perform() {
            beginTransaction(total_frame);

            player.getAnimator().CrossFade("SwordAttack2", 0.001f);
            player.lockInput(10);
            player.weapon.onAttack(5, 30, (hitbox, frame) => {
                hitbox.spec = spec;
            });
        }

        public override bool condition() {
            return !player.getAnimator().GetBool("Turn") && isFreeTransaction();
        }
    }
}