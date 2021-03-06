﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using DG.Tweening;

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

    public interface IEquipable {
        void onEquippedBy(Character ch);
        void onUnequippedBy(Character ch);
    }

    public interface IPassable {
        bool passing {
            get;
        }
    }

    public interface IOpenable {
        void open();
        void close();
        void terminate();
        bool authorized(object obj);
    }

    public interface IGUI {
        GameObject getGameObject();
    }

    public interface IGUIOpenable : IOpenable, IGUI {
    }

    public interface IGUIOpenableGroup : IGUIOpenable {
        List<IGUIOpenable> openables { get; }
    }

    public interface IGUIWorldSpaceUILinked {
        GameObject createWorldUI(Vector3 pos);
    }
}