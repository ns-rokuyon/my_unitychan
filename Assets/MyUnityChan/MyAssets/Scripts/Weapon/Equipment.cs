using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using UniRx;

namespace MyUnityChan {
    [RequireComponent(typeof(InteractionSystem))]
    public class Equipment : ObjectBase {

        // Current equipped object
        [ReadOnly]
        public InteractionObject interaction_object;

        private InteractionSystem interaction;
        private Character character;

        public bool acceptable {
            get {
                return interaction_object == null;
            }
        }

        void Awake() {
            interaction = GetComponent<InteractionSystem>();
            character = GetComponent<Character>();
        }

        public void equip(IPickupable pickupable, Const.ID.PickupSlot slot) {
            if ( interaction_object != null )
                return;
            interact(pickupable.interaction_object, slot);
            interaction_object = pickupable.interaction_object;

            if ( pickupable is IEquipable )
                (pickupable as IEquipable).onEquippedBy(character);
        }

        public InteractionObject unequip() {
            if ( interaction_object == null )
                return null;

            release(Const.ID.PickupSlot.LEFT_HAND);
            release(Const.ID.PickupSlot.RIGHT_HAND);

            var equipable = interaction_object.GetComponent<IEquipable>();
            if ( equipable != null )
                equipable.onUnequippedBy(character);

            var unequipped_obj = interaction_object;
            interaction_object = null;
            return unequipped_obj;
        }

        private void interact(InteractionObject itobj, Const.ID.PickupSlot slot) {
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
                    break;
            }
        }

        private void release(Const.ID.PickupSlot slot) {
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
        }
    }
}