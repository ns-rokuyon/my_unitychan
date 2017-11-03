using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class Missile : Projectile {

        public float driving_force = 1200f;
        public bool recoil = true;

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

            // Shoot
            Vector3 f = new Vector3(xdir * driving_force, 0, 0);
            rigid_body.AddForce(f);

            // Recoil
            shooter.owner.rigid_body.AddForce(f.flipX(), ForceMode.Impulse);
        }

        public override void initialize() {
            var collider = gameObject.GetComponentInChildren<Collider>();
            var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            var pss = GetComponentsInChildren<ParticleSystem>();
            var lights = GetComponentsInChildren<Light>();

            component_to_disable_in_waiting.Add(f => collider.enabled = f );
            component_to_disable_in_waiting.Add(f => renderers.ToList().ForEach(rd => rd.enabled = f));
            component_to_disable_in_waiting.Add(f => lights.ToList().ForEach(light => light.enabled = f));
            component_to_disable_in_waiting.Add(f => pss.ToList().ForEach(ps => {
                if ( f )
                    ps.Play();
                else
                    ps.Stop();
            }));

            penetration = false;
            distance_moved = 0.0f;
            hit_num = 0;
            waiting_for_destroying = false;
        }

        public override void finalize() {
            rigid_body.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            component_to_disable_in_waiting.Clear();
        }
    }
}
