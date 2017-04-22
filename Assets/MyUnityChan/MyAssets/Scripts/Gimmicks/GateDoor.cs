using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;

namespace MyUnityChan {
    public class GateDoor : Door, Lockable {
        public static Dictionary<Const.ID.DoorType, Material> mats { get; protected set; }

        public Const.ID.DoorType door_type;
        public Material mat {
            get {
                return GetComponent<Renderer>().material;
            }
            set {
                GetComponent<Renderer>().material = value;
            }
        }

        void Awake() {
            setupSoundPlayer();
            
            if ( mats == null || mats.Count == 0 ) {
                mats = new Dictionary<Const.ID.DoorType, Material>();
                mats.Add(Const.ID.DoorType.FULL_ACCESS,
                    Resources.Load("Materials/Common/Door", typeof(Material)) as Material);
                mats.Add(Const.ID.DoorType.LOCKED,
                    Resources.Load("Materials/Common/LockedDoor", typeof(Material)) as Material);
            }
        }

        void Start() {
            this.ObserveEveryValueChanged(_ => door_type)
                .Subscribe(dt => {
                    mat = mats[dt];
                });
        }

        public override void open() {
            if ( door_type == Const.ID.DoorType.LOCKED )
                return;
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            se(Const.ID.SE.GATE_OPEN);
        }

        public override void close() {
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }

        public void doLock() {
            door_type = Const.ID.DoorType.LOCKED;
        }

        public void doUnlock() {
            door_type = Const.ID.DoorType.FULL_ACCESS;
        }
    }
}
