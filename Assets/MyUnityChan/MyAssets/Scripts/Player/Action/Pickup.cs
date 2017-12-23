using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using System.Linq;

namespace MyUnityChan {
    public class PlayerPickup : PlayerAction {
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

        public override void perform() {
            player.lockInput(40);
            List<IPickupable> near_pickupables = player.GetComponentsInSameArea<IPickupable>().Where(w => w.canPickup).ToList();
            if ( near_pickupables.Count == 0 )
                return;

            // Nearest one
            IPickupable pickupable = near_pickupables.OrderBy(w => player.distanceXTo(w.position)).First();
            pickup(pickupable);
        }

        public override bool condition() {
            if ( player.hasEquipment() )
                return false;

            IPickupable[] near_pickupables = player.GetComponentsInSameArea<IPickupable>();
            if ( near_pickupables.Length == 0 )
                return false;
            int count = near_pickupables.ToList().Count(w => w.canPickup && !w.isOwned);
            return count > 0 && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }

        public void pickup(IPickupable pickupable) {
            if ( pickupable.interactionSlot == Const.ID.PickupSlot.RIGHT_HAND ) {
                player.equip(pickupable, Const.ID.PickupSlot.RIGHT_HAND);
            }
            else if ( pickupable.interactionSlot == Const.ID.PickupSlot.LEFT_HAND ) {
                player.equip(pickupable, Const.ID.PickupSlot.LEFT_HAND);
            }

            pickupable.onPickedUpBy(player);
        }
    }
}
