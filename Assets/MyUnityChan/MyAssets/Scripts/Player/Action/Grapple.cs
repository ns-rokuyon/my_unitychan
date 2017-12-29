using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public class PlayerGrapple : PlayerAction {
        public float max_distance { get; set; }
        public float radius { get; set; }
        public bool grappled { get; set; }

        public GameObject hook { get; set; }                // GameObject
        public GrapplingHook grapplinghook { get; set; }    // Component

        private RaycastHit ghit;

        // Bone refs
        private Transform right_arm;
        private Transform right_fore_arm;
        private Transform right_hand;

        public PlayerGrapple(Character ch) : base(ch) {
            radius = 0.2f;
            grappled = false;

            /* 
            Limit raycast distance.
            'max_distance' should be matched grappling length.
            (match to LocalPosition.y of Hand object in GrapplingHook prefab)
            */
            max_distance = 4.0f;

            // Register child action
            player.action_manager.registerAction(new PlayerSwing(ch));
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
                grapplinghook = hook.GetComponentInChildren<GrapplingHook>();
                grapplinghook.initPosition(ghit);
                grapplinghook.length = max_distance;
                grapplinghook.connect(player);
                player.ik.solver.leftFootEffector.target = player.leftfoot_anchor;
                player.ik.solver.leftFootEffector.positionWeight = 0.5f;
                player.ik.solver.leftFootEffector.Initiate(player.ik.solver);
                player.ik.solver.rightFootEffector.target = player.rightfoot_anchor;
                player.ik.solver.rightFootEffector.positionWeight = 0.5f;
                player.ik.solver.rightFootEffector.Initiate(player.ik.solver);
                grappled = true;
            }
        }

        public override void constant_performLate() {
            if ( !grappled )
                return;

            if ( player.isLookAhead() ) {
                right_arm.rotation = Quaternion.AngleAxis(250.0f, new Vector3(0, 0, 1));
                right_fore_arm.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0));
                right_hand.localRotation = Quaternion.AngleAxis(45.0f, new Vector3(0, 1, 0));
            }
            else {
                right_arm.rotation = Quaternion.AngleAxis(290.0f, new Vector3(0, 0, 1));
                right_fore_arm.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0));
                right_hand.localRotation = Quaternion.AngleAxis(-45.0f, new Vector3(0, 1, 0));
            }
        }

        public override void init() {
            player.action_manager.getAction("JUMP").perform_callbacks.Add(disconnect);

            right_arm = player.bone_manager.bone(Const.ID.UnityChanBone.RIGHT_ARM).transform;
            right_fore_arm = player.bone_manager.bone(Const.ID.UnityChanBone.RIGHT_FORE_ARM).transform;
            right_hand = player.bone_manager.bone(Const.ID.UnityChanBone.RIGHT_HAND).transform;
        }

        public void disconnect() {
            if ( hook ) {
                hook.GetComponentInChildren<GrapplingHook>().disconnect();
                hook = null;
                grapplinghook = null;
                player.ik.solver.leftFootEffector.target = null;
                player.ik.solver.leftFootEffector.positionWeight = 0.0f;
                player.ik.solver.leftFootEffector.Initiate(player.ik.solver);
                player.ik.solver.rightFootEffector.target = null;
                player.ik.solver.rightFootEffector.positionWeight = 0.0f;
                player.ik.solver.rightFootEffector.Initiate(player.ik.solver);
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

    public class PlayerSwing : PlayerAction {
        public float swing_deg_limit;   // Add swing force to player when hook degree is in this range

        private PlayerGrapple grapple;
        private Rigidbody rb;
        private Vector3 swingF;
        private Vector3 leftfoot_anchor_basepoint;
        private Vector3 rightfoot_anchor_basepoint;

        public PlayerSwing(Character ch) : base(ch) {
            grapple = null;
            rb = null;
            swingF = new Vector3(2000.0f, 0, 0);
            swing_deg_limit = 20.0f;
        }

        public override void init() {
            grapple = player.action_manager.getAction<PlayerGrapple>("GRAPPLE");
            rb = player.GetComponent<Rigidbody>();

            if ( player.leftfoot_anchor )
                leftfoot_anchor_basepoint = player.leftfoot_anchor.localPosition;
            if ( player.rightfoot_anchor )
                rightfoot_anchor_basepoint = player.rightfoot_anchor.localPosition;
        }

        public override void performFixed() {
            if ( rb == null )
                return;

            if ( grapple.grapplinghook == null )
                return;

            player.getAnimator().Play("Idle");

            float degree = grapple.grapplinghook.getSwingDegree();
            float swing_foot_offset_x = player.getFrontVector().x * grapple.grapplinghook.swing_dir * Mathf.Min(4.0f * (degree / 90.0f), 1.0f);
            float swing_foot_offset_y = Mathf.Min((degree / 90.0f), 1.0f);

            if ( player.ik.solver.leftFootEffector.target )
                player.leftfoot_anchor.localPosition = leftfoot_anchor_basepoint.add(0.0f, swing_foot_offset_y, swing_foot_offset_x);

            if ( player.ik.solver.rightFootEffector.target )
                player.rightfoot_anchor.localPosition = rightfoot_anchor_basepoint.add(0.0f, swing_foot_offset_y, swing_foot_offset_x);

            if ( degree > swing_deg_limit )
                return;

            var horizontal = controller.keyHorizontal();
            rb.AddForce(swingF * horizontal);
        }

        public override bool condition() {
            if ( grapple == null ) {
                init();
                return false;
            }
            return grapple.grappled;
        }

        public override Const.PlayerAction id() {
            return Const.PlayerAction.SWING;
        }

        public override string name() {
            return "SWING";
        }
    }
}