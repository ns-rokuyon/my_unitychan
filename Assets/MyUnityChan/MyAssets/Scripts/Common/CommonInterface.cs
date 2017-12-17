using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

namespace MyUnityChan {
    public interface Lockable {
        void doLock();
        void doUnlock();
    }

    public interface IPickupable {
        bool canPickup {
            get;
        }
        bool isOwned {
            get;
        }
        Vector3 position {
            get;
        }
        InteractionObject interaction_object {
            get;
        }
        Const.ID.PickupSlot interactionSlot {
            get;
        }
        List<HandTarget> followHandTargets {
            get;
        }

        void onPickedUpBy(Character ch);
        void onThrownOutBy(Character ch, float throw_fx);
    }
}