using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace MyUnityChan {
    public class PSEmissionEaser : ObjectBase {
        public float emission_power {
            get {
                return ps.emission.rateOverTime.constant;
            }
            set {
                var em = ps.emission;
                em.rateOverTime = new ParticleSystem.MinMaxCurve(value);
            }
        }

        public float normalized_emission_power {
            get {
                if ( initial_emission_power <= 0.0f )
                    return 0.0f;
                float p = emission_power / initial_emission_power;
                return p >= 1.0f ? 1.0f : p;
            }
            set {
                float p = value * initial_emission_power;
                emission_power = p >= 1.0f ? initial_emission_power : p;
            }
        }

        public ParticleSystem ps { get; protected set; }
        public float initial_emission_power { get; protected set; }

        void Awake() {
            ps = GetComponent<ParticleSystem>();
            initial_emission_power = ps.emission.rateOverTime.constant;
        }

        public void powerOn(float easing_time = 0.0f) {
            if ( easing_time > 0.0f ) {
                DOTween.To(() => normalized_emission_power,
                           (x) => normalized_emission_power = x,
                           1.0f,
                           easing_time);
            } else {
                normalized_emission_power = 1.0f;
            }
        }

        public void powerOff(float easing_time = 0.0f) {
            if ( easing_time > 0.0f ) {
                DOTween.To(() => normalized_emission_power,
                           (x) => normalized_emission_power = x,
                           0.0f,
                           easing_time);
            } else {
                normalized_emission_power = 0.0f;
            }
        }
    }
}