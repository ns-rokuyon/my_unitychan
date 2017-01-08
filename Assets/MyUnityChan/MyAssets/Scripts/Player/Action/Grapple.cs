using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerGrapple : PlayerAction {
        public float max_distance { get; set; }
        public float radius { get; set; }
        public bool grappled { get; set; }
        public GameObject hook { get; set; }

        private RaycastHit ghit;

        public PlayerGrapple(Character ch) : base(ch) {
            radius = 0.2f;
            grappled = false;

            /* 
            Limit raycast distance.
            'max_distance' should be matched grappling length.
            (match to LocalPosition.y of Hand object in GrapplingHook prefab)
            */
            max_distance = 4.0f;    
        }

        public bool raycast() {
            return Physics.SphereCast(player.transform.position,
                                      radius, player.getFrontVector().changeY(1.0f),
                                      out ghit, max_distance);
        }

        public override void perform() {
            bool success = raycast();
            if ( !grappled && success ) {
                hook = PrefabInstantiater.create(Const.Prefab.Gimmick[Const.ID.Gimmick.GRAPPLING_HOOK]);
                hook.transform.position = ghit.point;
                hook.GetComponentInChildren<GrapplingHook>().length = max_distance;
                hook.GetComponentInChildren<GrapplingHook>().connect(player);
                grappled = true;
            }
        }

        public override void init() {
            player.action_manager.getAction("JUMP").perform_callbacks.Add(disconnect);
        }

        public void disconnect() {
            if ( hook ) {
                hook.GetComponentInChildren<GrapplingHook>().disconnect();
                hook = null;
                grappled = false;
            }
        }

        public override bool condition() {
            return controller.keyGrapple() && !player.isFrozen();
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.GRAPPLE;
        }

        public override string name() {
            return "GRAPPLE";
        }
    }
}