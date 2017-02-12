using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

namespace MyUnityChan {
    public class PlayerDemo : ObjectBase {
        public PlayerManager pm { get; protected set; }
        public IDisposable demo { get; protected set; }
        public Dictionary<Const.CharacterName, Vector3> center_positions { get; protected set; }
        public ModelViewCamera demo_camera { get; protected set; }

        void Awake() {
            pm = GetComponentInChildren<PlayerManager>();
            center_positions = new Dictionary<Const.CharacterName, Vector3>();
            demo_camera = GetComponentInChildren<ModelViewCamera>();
        }

        void Start() {
            foreach ( var kv in pm.players ) {
                center_positions.Add(kv.Key, kv.Value.transform.position);
            }
        }

        public void play(Func<IObserver<Controller.InputCode>, IEnumerator> co) {
            if ( demo != null )
                demo.Dispose();

            demo = Observable.FromCoroutine<Controller.InputCode>(observer => co(observer))
                .Subscribe(code => pm.controller.inputKey(code, frame: 10));
        }

        public void centering() {
            foreach ( var kv in pm.players ) {
                kv.Value.gameObject.transform.position = center_positions[kv.Key];
            }
        }

        public void setDemoCameraDistance(float z) {
            demo_camera.distance = z;
            demo_camera.setStartPosition();
        }
    }
}