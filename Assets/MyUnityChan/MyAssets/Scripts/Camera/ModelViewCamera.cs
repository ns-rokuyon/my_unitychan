using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using AmplifyColor;
using System;

namespace MyUnityChan {
    public class ModelViewCamera : ObjectBase, IOpenable {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private bool orbit;

        [SerializeField]
        private bool orbital_reverse;

        [SerializeField]
        private float orbital_speed_scaling = 40.0f;

        [SerializeField]
        private Texture focused_lut;

        [SerializeField]
        private Texture unfocused_lut;

        private Camera _camera;
        public AmplifyColorEffect color_effect { get; private set; }
        public float distance { get; set; }
        public bool no_target { get; set; }
        public PlayerManager pm { get; protected set; }
        public Vector3 first_position { get; protected set; }

        public Vector3 orbital_center_point {
            get {
                return target.transform.position.changeY(transform.position.y);
            }
        }

        public float angular_velocity {
            get {
                return (orbital_reverse ? -1.0f : 1.0f) * orbital_speed_scaling * time_control.deltaTime;
            }
        }

        void Awake() {
            first_position = transform.position;
            color_effect = GetComponent<AmplifyColorEffect>();

            if ( target ) {
                pm = target.GetComponent<PlayerManager>();
            }
            else {
                no_target = true;
            }
        }

        void Start() {
            if ( !target ) return;

            setStartPosition();

            this.LateUpdateAsObservable()
                .Where(_ => orbit && !no_target)
                .Subscribe(_ => {
                    transform.RotateAround(orbital_center_point, Vector3.up, angular_velocity);
                    transform.LookAt(orbital_center_point);
                }).
                AddTo(this);
        }

        public void setStartPosition() {
        }

        public Camera getCamera() {
            if ( !_camera )
                _camera = GetComponent<Camera>();
            return _camera;
        }

        public void open() {
            if ( focused_lut )
                color_effect.LutTexture = focused_lut;
        }

        public void close() {
            if ( unfocused_lut )
                color_effect.LutTexture = unfocused_lut;
        }

        public void terminate() {
            close();
        }

        public bool authorized(object obj) {
            return true;
        }
    }
}
