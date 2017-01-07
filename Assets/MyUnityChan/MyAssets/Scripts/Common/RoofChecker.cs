using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class RoofChecker : GroundChecker {
        // Use this for initialization
        void Start() {
            radius = transform.lossyScale.x * sphere_scale;
            shift = Vector3.down * (radius + delta + start_y_offset);
        }

        public bool isHitRoof() {
            return Physics.SphereCast(transform.position - shift,
                                      radius, Vector3.up, out ghit, max_distance);
        }

        void OnDrawGizmos() {
            var r = transform.lossyScale.x * sphere_scale;
            var s = Vector3.down * (r + delta + start_y_offset);
            Gizmos.DrawRay(transform.position - s, Vector3.up * max_distance);
            if ( isHitRoof() ) 
                Gizmos.DrawWireSphere(ghit.point, radius);
        }
    }
}