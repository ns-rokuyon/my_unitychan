using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class Lift : MovingFloor {
        public List<Vector3> points;
        public float speed;

        private bool no_move;
        private int points_num;
        private int direction;
        private float distance_to_endpoint;
        private float distance_travelled;
        private Vector3 distance_move;      // move distance a frame

        // Use this for initialization
        void Start() {
            if ( points.Count == 0 ) {
                no_move = true;
                return;
            }
            else if ( points.Count == 1 ) {
                no_move = true;
                transform.position = points[0];
                return;
            }

            transform.position = points[0];
            points_num = points.Count;
            direction = 1;

            calcMove(points[0], points[direction]);

            distance_travelled = 0.0f;
            distance_to_endpoint = Vector3.Distance(points[0], points[direction]);
        }

        // Update is called once per frame
        void Update() {
            if ( !no_move ) {
                Vector3 old_pos = transform.position;

                // update position
                transform.position += distance_move;

                distance_travelled += Vector3.Distance(old_pos, transform.position);
                updateDirection();
            }
        }

        private void calcMove(Vector3 start, Vector3 end) {
            float new_vx = 0.0f;
            float new_vy = 0.0f;
            if ( start.x < end.x ) {
                new_vx = speed;
            }
            else if ( start.x > end.x ) {
                new_vx = -speed;
            }
            else {
                new_vx = 0.0f;
            }

            if ( start.y < end.y ) {
                new_vy = speed;
            }
            else if ( start.y > end.y ) {
                new_vy = -speed;
            }
            else {
                new_vy = 0.0f;
            }

            distance_move = new Vector3(new_vx, new_vy, 0.0f);

        }

        private void updateDirection() {
            if ( distance_travelled >= distance_to_endpoint ) {
                Vector3 thispoint = points[direction];

                if ( points_num - 1 == direction ) {
                    direction = 0;
                }
                else {
                    direction++;
                }

                distance_travelled = 0.0f;
                distance_to_endpoint = Vector3.Distance(thispoint, points[direction]);

                calcMove(thispoint, points[direction]);

            }
        }
    }
}
