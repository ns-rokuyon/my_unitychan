using UnityEngine;
using System;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    [RequireComponent(typeof(LineRenderer))]
    public class GrapplingHook : ObjectBase {
        public GameObject hook;
        public GameObject hand;

        private float _length;
        public float length {
            get {
                return _length;
            }
            set {
                _length = value;
                if ( hand )
                    hand.transform.localPosition = -hand.transform.localPosition.changeY(length);
            }
        }
        public LineRenderer line { get; set; }
        public Player player { get; set; }
        public SpringJoint joint { get; set; }
        public IDisposable position_updater { get; set; }

        void Awake() {
            line = GetComponent<LineRenderer>();
        }

        void Update() {
            line.SetPosition(0, hook.transform.position);
            if ( player )
                line.SetPosition(1, player.bone_manager.position(Const.ID.UnityChanBone.RIGHT_HAND));
            else
                line.SetPosition(1, hand.transform.position);
        }

        public void initPosition(RaycastHit hit) {
            var obj = hit.collider.gameObject;
            var character = obj.GetComponent<Character>();
            if ( character ) {
                position_updater = Observable.EveryUpdate()
                    .Subscribe(_ => hook.transform.position = obj.transform.position);
            }
            else {
                hook.transform.position = hit.point;
                position_updater = null;
            }
        }

        public void connect(Player _player) {
            player = _player;
            hand.transform.position = player.transform.position;
            hand.AddComponent<SpringJoint>();
            joint = hand.GetComponent<SpringJoint>();
            joint.connectedBody = player.rigid_body;
            joint.spring = 5000;
            joint.damper = 0;
        }

        public void disconnect() {
            if ( position_updater != null ) {
                position_updater.Dispose();
                position_updater = null;
            }

            joint.connectedBody = null;
            Destroy(transform.parent.gameObject);
        }

        public float getSwingDegree() {
            Vector3 basepoint = hook.transform.position;
            Vector3 handpoint = line.GetPosition(1);
            return Mathf.Atan2(Mathf.Abs(basepoint.x - handpoint.x),
                               Mathf.Abs(basepoint.y - handpoint.y)) * Mathf.Rad2Deg;
        }
    }
}