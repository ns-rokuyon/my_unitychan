using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Flame : Projectile {
        private TimerState lifetime = null;
        private int lifetime_frame = 180;
        private DamageObjectHitbox hitbox = null;

        private ParticleSystem particle_system = null;
        private Light light = null;

        // Use this for initialization
        void Start() {
            initialize();
        }

        // Update is called once per frame
        void Update() {
            if ( lifetime != null ) {
                if ( lifetime.isFinished() ) {
                    ObjectPoolManager.releaseGameObject(this.gameObject, Const.Prefab.Projectile["FLAME"]);
                }

                if ( lifetime.getTimer() != null && ((FrameTimer)lifetime.getTimer()).now() > 120 ) {
                    fadeOut();
                }
            }
        }

        private void fadeOut() {
            Color color = particle_system.startColor;
            particle_system.startColor = new Color(color.r, color.g, color.b, color.a - 5);

            light.intensity -= 0.1f;
            if ( light.intensity <= 0.0f ) {
                light.intensity = 0.0f;
            }
        }

        public override void initialize() {
            //createHitbox();

            particle_system = this.gameObject.GetComponent<ParticleSystem>();
            Color color = particle_system.startColor;
            particle_system.startColor = new Color(color.r, color.g, color.b, 255);

            light = this.gameObject.GetComponentsInChildren<Light>(true)[0];
            light.intensity = 1.5f;

            lifetime = new FrameTimerState();
            lifetime.createTimer(lifetime_frame);
        }

        public override void finalize() {
        }

        private void createHitbox() {
            hitbox = HitboxManager.self().create<DamageObjectHitbox>(Const.Prefab.Hitbox["FLAME"], true);
            hitbox.ready(this.gameObject, spec);
        }

        public DamageObjectHitbox getHitbox() {
            return hitbox;
        }

    }
}
