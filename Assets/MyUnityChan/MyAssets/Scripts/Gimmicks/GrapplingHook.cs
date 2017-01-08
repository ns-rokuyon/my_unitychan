using UnityEngine;
using System.Collections;

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

        void Awake() {
            line = GetComponent<LineRenderer>();
        }

        void Update() {
            line.SetPosition(0, hook.transform.position);
            if ( player )
                line.SetPosition(1, player.transform.position);
            else
                line.SetPosition(1, hand.transform.position);
        }

        public void connect(Player _player) {
            player = _player;
            hand.transform.position = player.transform.position;
            hand.AddComponent<SpringJoint>();
            joint = hand.GetComponent<SpringJoint>();
            joint.connectedBody = player.rigid_body;
            joint.spring = 1500;
            joint.damper = 2000;
        }

        public void disconnect() {
            joint.connectedBody = null;
            Destroy(transform.parent.gameObject);
        }
    }
}