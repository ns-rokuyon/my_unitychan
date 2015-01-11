using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ParticleEffect : EffectBase {
        private ParticleSystem particle_system;

        // Use this for initialization
        void Start() {
            particle_system = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update() {
            if ( !particle_system.IsAlive() ) {
                Destroy(this.gameObject);
            }
        }
    }
}
