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

        public Vector3 upshift { get; set; }
        public float radius { get; set; }

        private RaycastHit ghit;

        // Use this for initialization
        void Start() {
            radius = transform.lossyScale.x * sphere_scale;
            upshift = Vector3.up * (radius + delta + start_y_offset);
        }

        public bool isGrounded() {
            return Physics.SphereCast(transform.position + upshift,
                                      radius, Vector3.down, out ghit, max_distance);
        }

        public Vector3 point() {
            return ghit.point;
        }

        void OnDrawGizmos() {
            Gizmos.DrawRay(transform.position + upshift, Vector3.down * max_distance);
            if ( isGrounded() ) 
                Gizmos.DrawWireSphere(ghit.point, radius);
        }
    }
}