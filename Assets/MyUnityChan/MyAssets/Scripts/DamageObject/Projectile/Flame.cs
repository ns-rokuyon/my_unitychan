using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class Flame : Projectile {
        public bool fading_out { get; set; }
        private int lifetime_frame = 180;
        private DamageObjectHitbox hitbox = null;

        private ParticleSystem particle_system = null;
        private Light light = null;

        // Use this for initialization
        void Start() {
            initialize();
        }

        private void destroy() {
            ObjectPoolManager.releaseGameObject(this.gameObject, Const.Prefab.Projectile["FLAME"]);
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
            fading_out = false;

            particle_system = this.gameObject.GetComponent<ParticleSystem>();
            Color color = particle_system.startColor;
            particle_system.startColor = new Color(color.r, color.g, color.b, 255);

            light = this.gameObject.GetComponentsInChildren<Light>(true)[0];
            light.intensity = 1.5f;

            Observable.EveryUpdate()
                .Where(_ => fading_out)
                .Subscribe(_ => fadeOut());

            this.ObserveEveryValueChanged(_ => light.intensity)
                .Where(v => v <= 0.0)
                .Single()
                .Subscribe(_ => destroy());
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
