using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class AreaGate : ObjectBase {
        public GameObject gate_pair;

        public bool pass { get; set; }
        public GateDoor door { get; protected set; }
        public GameObject dst { get; protected set; }
        public GameObject entrance { get; protected set; }
        public GameObject active_holo { get; protected set; }

        void Awake() {
            pass = false;
            door = GetComponentInChildren<GateDoor>();

            entrance = this.transform.Find("GateDestination").gameObject;

            if ( gate_pair ) {
                dst = gate_pair.transform.Find("GateDestination").gameObject;

                // set destination to gate start collision
                var warp = transform.Find("GateStart").gameObject.GetComponent<Warp>();
                warp.warp_to = dst;
                (warp as AreaGateWarpCollision).gate = this;
            }
        }

        void Start() {
            if ( gate_pair )
                AreaManager.self().registerAreaConnectionInfo(this.gameObject, gate_pair);
            else
                DebugManager.warn("No pair gate");

            if ( door ) {
                this.ObserveEveryValueChanged(_ => door.door_type)
                    .Subscribe(dt => {
                        controlHolo(dt);
                    });
            }
        }

        public void onPass() {
            pass = true;
            if ( gate_pair )
                gate_pair.GetComponent<AreaGate>().pass = true;
        }

        protected void controlHolo(Const.ID.DoorType dt) {
            switch ( dt ) {
                case Const.ID.DoorType.LOCKED:
                    {
                        if ( !active_holo ) {
                            active_holo = PrefabInstantiater.create(Const.Prefab.UI["HOLO_LOCKED"],
                                GUIObjectBase.getCanvas(Const.Canvas.WORLD_SPACE_CANVAS));
                            active_holo.transform.position = entrance.transform.position.add(0, 0, 1.5f);
                            delay(120, () => { active_holo.GetComponent<Hologram>().appear(); });
                        }
                        break;
                    }
                default:
                    {
                        if ( active_holo ) {
                            delay(10, () => {
                                active_holo.GetComponent<Hologram>().disappear();
                                delay(120, () => {
                                    Destroy(active_holo);
                                    active_holo = null;
                                });
                            });
                        }
                        break;
                    }
            }
        }
    }
}
