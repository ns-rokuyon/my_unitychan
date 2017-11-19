using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ParticleEffect : Effect {
        private ParticleSystem _particle_system;
        public ParticleSystem particle_system {
            get { return _particle_system ?? (_particle_system = GetComponent<ParticleSystem>()); }
        }

        // Update is called once per frame
        void Update() {
            if ( !particle_system.IsAlive() ) {
                Destroy(this.gameObject);
            }
        }
    }
}
