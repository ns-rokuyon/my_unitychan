using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace MyUnityChan {
    public class ModelViewCamera : ObjectBase {
        public GameObject target;
        public float distance;
        public Vector3 angle;
        public Vector3 offset;
        public bool orbit;
        public bool orbital_reverse;
        public float orbital_speed_scaling = 40.0f;

        private Camera _camera;

        public Vector3 target_position_offseted {
            get {
                return target.transform.position.add(offset.x, offset.y, offset.z);
            }
        }

        public float angular_velocity {
            get {
                return (orbital_reverse ? -1.0f : 1.0f) * orbital_speed_scaling * (tc ? tc.deltaTime : Time.deltaTime);
            }
        }

        public TimeControllable tc { get; protected set; }
        public PlayerManager pm { get; protected set; }
        public Vector3 first_position { get; protected set; }

        void Awake() {
            tc = GetComponent<TimeControllable>();
            if ( orbit && !tc ) {
                DebugManager.warn("Orbit camera requires TimeControllable component");
            }
            pm = GetComponent<PlayerManager>();
        }

        void Start() {
            if ( !target ) return;

            setStartPosition();

            if ( orbit ) {
                Observable.EveryLateUpdate()
                    .Where(_ => orbit)
                    .Subscribe(_ => {
                        transform.RotateAround(target_position_offseted, Vector3.up, angular_velocity);
                        transform.LookAt(target_position_offseted);
                    });
            }
        }

        public void setStartPosition() {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - distance);
            transform.position = transform.position.add(offset.x, offset.y, offset.z);
            gameObject.transform.rotation = Quaternion.Euler(angle);
        }

        public Camera getCamera() {
            if ( !_camera )
                _camera = GetComponent<Camera>();
            return _camera;
        }

    }
}
