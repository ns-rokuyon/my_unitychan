using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;


namespace MyUnityChan {
    [RequireComponent(typeof(FullBodyBipedIK))]
    public class PlayerIK : ObjectBase {
        public Transform body_anchor;
        public Transform leftfoot_anchor;
        public Transform rightfoot_anchor;

        public FullBodyBipedIK ik { get; private set; }

        // Base local positions
        public Vector3 body_anchor_basepoint { get; private set; }
        public Vector3 leftfoot_anchor_basepoint { get; private set; }
        public Vector3 rightfoot_anchor_basepoint { get; private set; }

        void Awake() {
            ik = GetComponent<FullBodyBipedIK>();

            body_anchor_basepoint = body_anchor.localPosition;
            leftfoot_anchor_basepoint = leftfoot_anchor.localPosition;
            rightfoot_anchor_basepoint = rightfoot_anchor.localPosition;
        }

        // Set IK position at anchor transform
        public void bind(Const.ID.IKEffectorType ik_type, Transform anchor,
                         float position_weight = 1.0f,
                         float rotation_weight = 1.0f,
                         int frame = 0) {
            IKEffector effector = getIKEffector(ik_type);

            effector.target = anchor;
            effector.positionWeight = position_weight;
            if ( ik_type != Const.ID.IKEffectorType.BODY )
                effector.rotationWeight = rotation_weight;

            effector.Initiate(ik.solver);

            if ( frame > 0 )
                delay("bind#" + ik_type.ToString(), frame, () => debind(ik_type));
        }
        
        // Debind IK position
        public void debind(Const.ID.IKEffectorType ik_type) {
            IKEffector effector = getIKEffector(ik_type);

            effector.target = null;
            effector.positionWeight = 0.0f;
            if ( ik_type != Const.ID.IKEffectorType.BODY )
                effector.rotationWeight = 0.0f;

            effector.Initiate(ik.solver);
        }

        // Check now binding
        public bool isBinding(Const.ID.IKEffectorType ik_type, Transform tf = null) {
            IKEffector effector = getIKEffector(ik_type);

            if ( tf == null )
                return effector.target != null;
            return effector.target != null && effector.target == tf;
        }

        private IKEffector getIKEffector(Const.ID.IKEffectorType ik_type) {
            switch (ik_type) {
                case Const.ID.IKEffectorType.BODY: { return ik.solver.bodyEffector; }
                case Const.ID.IKEffectorType.LEFT_HAND: { return ik.solver.leftHandEffector; }
                case Const.ID.IKEffectorType.LEFT_FOOT: { return ik.solver.leftFootEffector; }
                case Const.ID.IKEffectorType.RIGHT_HAND: { return ik.solver.rightHandEffector; }
                case Const.ID.IKEffectorType.RIGHT_FOOT: { return ik.solver.rightFootEffector; }
            }
            return null;
        }
    }
}