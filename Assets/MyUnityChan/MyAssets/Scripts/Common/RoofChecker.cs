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

        public override float getDistance() {
            RaycastHit _ghit;
            Physics.SphereCast(transform.position + shift, radius, Vector3.up, out _ghit, 100.0f);
            return Vector3.Distance(transform.position, _ghit.point);
        }

        public override Vector3 getSpacingPoint(float space) {
            RaycastHit _ghit;
            Physics.SphereCast(transform.position + shift, radius, Vector3.up, out _ghit, 100.0f);
            return _ghit.point.sub(0.0f, space, 0.0f);
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