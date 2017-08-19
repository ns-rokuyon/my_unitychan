using UnityEngine;
using System.Collections;
using Chronos;

namespace MyUnityChan {
    public class RigidbodyWrapper : StructBase {
        public Rigidbody rb { get; protected set; }

        public RigidbodyWrapper(GameObject obj) {
            rb = obj.GetComponent<Rigidbody>();
        }

        public virtual float mass {
            get { return rb.mass; }
            set { rb.mass = value; }
        }

        public virtual Vector3 velocity {
            get { return rb.velocity; }
            set { rb.velocity = value; }
        }

        public virtual Vector3 angularVelocity {
            get { return rb.angularVelocity; }
            set { rb.angularVelocity = value; }
        }

        public virtual bool isKinematic {
            get { return rb.isKinematic; }
            set { rb.isKinematic = value; }
        }

		public virtual void AddForce(Vector3 force, ForceMode mode = ForceMode.Force) {
            rb.AddForce(force, mode);
        }

        public static implicit operator Rigidbody(RigidbodyWrapper t) {
            return t.rb;
        }

        public static implicit operator bool(RigidbodyWrapper t) {
            return t != null && t.rb != null;
        }
    }

    public class ChronosRigidbodyWrapper : RigidbodyWrapper {
        public RigidbodyTimeline3D crb { get; protected set; }

        public ChronosRigidbodyWrapper(GameObject obj) : base(obj) {
            var timecontrol = obj.GetComponent<ChronosTimeControllable>();
            if ( timecontrol ) {
                crb = timecontrol.timeline.rigidbody;
            }
        }

        public override float mass {
            get { return crb.mass; }
            set { crb.mass = value; }
        }

        public override Vector3 velocity {
            get { return crb.velocity; }
            set { crb.velocity = value; }
        }

        public override Vector3 angularVelocity {
            get { return crb.angularVelocity; }
            set { crb.angularVelocity = value; }
        }

        public override bool isKinematic {
            get { return crb.isKinematic; }
            set { crb.isKinematic = value; }
        }

		public override void AddForce(Vector3 force, ForceMode mode = ForceMode.Force) {
            crb.AddForce(force, mode);
        }
    }
}