using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using System.Linq;

namespace MyUnityChan {
    public class PlayerPickup : PlayerAction {
        private InteractionSystem interaction;

        public PlayerPickup(Character character)
            : base(character) {
            priority = 5;
            skip_lower_priority = true;
        }

        public override string name() {
            return "PICKUP";
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.PICKUP;
        }

        public override void init() {
            interaction = player.GetComponent<InteractionSystem>();
        }

        public override void perform() {
            player.lockInput(40);
            List<Weapon> near_weapons = player.GetComponentsInSameArea<Weapon>().Where(w => w.canPickup).ToList();
            if ( near_weapons.Count == 0 )
                return;

            // Nearest one
            Weapon weapon = near_weapons.OrderBy(w => player.distanceXTo(w.transform.position)).First();

            if ( weapon.pickup_slots.Contains(Const.ID.PickupSlot.RIGHT_HAND) ) {
                interaction.StartInteraction(FullBodyBipedEffector.RightHand, 
                    weapon.GetComponent<InteractionObject>(), true);
            }
            if ( weapon.pickup_slots.Contains(Const.ID.PickupSlot.LEFT_HAND) ) {
                interaction.StartInteraction(FullBodyBipedEffector.LeftHand, 
                    weapon.GetComponent<InteractionObject>(), true);
            }
            weapon.pickup(player);
        }

        public override bool condition() {
            Weapon[] near_weapons = player.GetComponentsInSameArea<Weapon>();
            if ( near_weapons.Length == 0 )
                return false;
            int count = near_weapons.ToList().Count(w => w.canPickup && !w.isOwned);
            return count > 0 && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }
    }
}
