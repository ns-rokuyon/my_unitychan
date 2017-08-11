using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class GroundChecker : ObjectBase {
        public float max_distance = 0.2f;
        public float sphere_scale = 0.05f;
        public float start_y_offset = 0.0f;
        public float delta = 0.01f;

        /*
            ^ Y
            |  radius
            |   ( )
            |    + ---------- transform.position + radius + delta + start_y_offset  (= Raycast start point)
            |    + ---------- transform.position + radius + delta
            |
            |    + ---------- transform.position
            |
           =|===(o)====================================================== Ground
            |
            |    + ---------- Raycast start point - max_distance    (= Raycast destination)
            |
            |

        */

        public Vector3 shift { get; set; }
        public float radius { get; set; }

        protected RaycastHit ghit;

        // Use this for initialization
        void Start() {
            radius = transform.lossyScale.x * sphere_scale;
            shift = Vector3.up * (radius + delta + start_y_offset);
        }

        public bool isGrounded() {
            return Physics.SphereCast(transform.position + shift,
                                      radius, Vector3.down, out ghit, max_distance);
        }

        public Vector3 point() {
            return ghit.point;
        }

        public virtual float getDistance() {
            RaycastHit _ghit;
            int ground_layer = 1 << LayerMask.NameToLayer("Ground");
            Physics.SphereCast(transform.position + shift, radius, Vector3.down, out _ghit, 100.0f, ground_layer);
            return Vector3.Distance(transform.position, _ghit.point);
        }

        public virtual Vector3 getSpacingPoint(float space) {
            RaycastHit _ghit;
            int ground_layer = 1 << LayerMask.NameToLayer("Ground");
            Physics.SphereCast(transform.position + shift, radius, Vector3.down, out _ghit, 100.0f, ground_layer);
            return _ghit.point.add(0.0f, space, 0.0f);
        }

        void OnDrawGizmos() {
            Gizmos.DrawRay(transform.position + shift, Vector3.down * max_distance);
            if ( isGrounded() ) 
                Gizmos.DrawWireSphere(ghit.point, radius);
        }
    }
}