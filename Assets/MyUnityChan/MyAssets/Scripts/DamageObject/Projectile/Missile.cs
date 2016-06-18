using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class Missile : Projectile {

        protected Rigidbody rigid_body;

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            projectileCommonUpdate();
        }

        public void fire(Vector3 start_point, float xdir) {
            foreach ( var component_enabler in component_to_disable_in_waiting ) {
                component_enabler(true);
            }

            setStartPosition(start_point.add(0, 1.0f, 0));
            if ( xdir < 0 ) transform.localRotation = Quaternion.Euler(0, 180f, 0);
            rigid_body.AddForce(new Vector3(xdir * 500.0f, 0, 0));
        }

        public override void initialize() {
            component_to_disable_in_waiting.Add(
                (bool f) => gameObject.GetComponentInChildren<Collider>().enabled = f );
            component_to_disable_in_waiting.Add(
                (bool f) => {
                    foreach ( var renderer in gameObject.GetComponentsInChildren<MeshRenderer>() ) {
                        renderer.enabled = f;
                    }
                });
            component_to_disable_in_waiting.Add(
                (bool f) => gameObject.GetComponentInChildren<ParticleSystem>().enableEmission = f );
            component_to_disable_in_waiting.Add(
                (bool f) => gameObject.GetComponentInChildren<Light>().enabled = f );

            penetration = false;
            distance_moved = 0.0f;
            hit_num = 0;
            waiting_for_destroying = false;
            rigid_body = GetComponent<Rigidbody>();
        }

        public override void finalize() {
            rigid_body.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            component_to_disable_in_waiting.Clear();
        }
    }
}
