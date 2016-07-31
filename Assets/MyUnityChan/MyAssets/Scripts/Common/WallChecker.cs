using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class WallChecker : ObjectBase {
        public float max_distance = 0.2f;
        public float sphere_scale = 0.05f;
        public float start_x_offset = 0.0f;
        public float start_y_offset = 0.0f;
        public float delta = 0.01f;

        public Vector3 frontshift {
            get {
                return character.getBackVector() * (radius + delta);
            }
        }
        public Vector3 upshift {
            get {
                return Vector3.up * start_y_offset;
            }
        }
        public float radius { get; set; }

        private RaycastHit ghit;
        private Character character;

        // Use this for initialization
        void Start() {
            radius = transform.lossyScale.x * sphere_scale;
            character = GetComponent<Character>();
        }

        public bool isTouchedWall() {
            return Physics.SphereCast(transform.position + frontshift,
                                      radius, character.getFrontVector(), out ghit, max_distance);
        }

        public Vector3 point() {
            return ghit.point;
        }

        void OnDrawGizmos() {
            Gizmos.DrawRay(transform.position + upshift + Vector3.right * (radius + delta), Vector3.right * max_distance);
            if ( Physics.SphereCast(transform.position + upshift + Vector3.right * (radius + delta),
                                      radius, Vector3.right, out ghit, max_distance)) {
                Gizmos.DrawWireSphere(ghit.point, radius);
            }
        }
    }
}
