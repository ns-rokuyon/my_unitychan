using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class LightingZone : SensorZone {
        [SerializeField] public Light target;
        [SerializeField] public LightParameter applied_light_param;

        public LightParameter default_light_param { get; protected set; }

        void Awake() {
            if ( target )
                default_light_param = new LightParameter(target);
        }

        void Start() {
            if ( target ) {
                onPlayerEnterCallback = (player, collider) => {
                    applied_light_param.apply(target);
                };

                onPlayerExitCallback = (player, collider) => {
                    default_light_param.apply(target);
                };
            }
        }
    }
}