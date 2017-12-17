using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using System.Linq;

namespace MyUnityChan {
    public class PlayerPickup : PlayerAction {
        private InteractionSystem interaction;
        private InteractionObject interaction_object;

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
            List<IPickupable> near_pickupables = player.GetComponentsInSameArea<IPickupable>().Where(w => w.canPickup).ToList();
            if ( near_pickupables.Count == 0 )
                return;

            // Nearest one
            IPickupable pickupable = near_pickupables.OrderBy(w => player.distanceXTo(w.position)).First();

            if ( pickupable.interactionSlot == Const.ID.PickupSlot.RIGHT_HAND ) {
                interact(pickupable.interaction_object, Const.ID.PickupSlot.RIGHT_HAND);
            }
            else if ( pickupable.interactionSlot == Const.ID.PickupSlot.LEFT_HAND ) {
                interact(pickupable.interaction_object, Const.ID.PickupSlot.LEFT_HAND);
            }

            pickupable.onPickedUpBy(player);
        }

        public override bool condition() {
            IPickupable[] near_pickupables = player.GetComponentsInSameArea<IPickupable>();
            if ( near_pickupables.Length == 0 )
                return false;
            int count = near_pickupables.ToList().Count(w => w.canPickup && !w.isOwned);
            return count > 0 && controller.keyAttack() && !player.getAnimator().GetBool("Turn") && player.isGrounded();
        }

        public bool interact(InteractionObject itobj, Const.ID.PickupSlot slot) {
            if ( interaction_object != null )
                return false;

            switch ( slot ) {
                case Const.ID.PickupSlot.LEFT_HAND:
                    {
                        interaction.StartInteraction(FullBodyBipedEffector.LeftHand, itobj, true);
                        break;
                    }
                case Const.ID.PickupSlot.RIGHT_HAND:
                    {
                        interaction.StartInteraction(FullBodyBipedEffector.RightHand, itobj, true);
                        break;
                    }
                default:
                    return false;
            }
            interaction_object = itobj;
            return true;
        }

        public void release(Const.ID.PickupSlot slot) {
            if ( interaction_object == null )
                return;
            
            switch ( slot ) {
                case Const.ID.PickupSlot.LEFT_HAND:
                    {
                        interaction.StopInteraction(FullBodyBipedEffector.LeftHand);
                        break;
                    }
                case Const.ID.PickupSlot.RIGHT_HAND:
                    {
                        interaction.StopInteraction(FullBodyBipedEffector.RightHand);
                        break;
                    }
                default:
                    return;
            }
            interaction_object = null;
        }
    }
}
