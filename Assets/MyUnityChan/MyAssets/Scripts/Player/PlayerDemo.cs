using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RootMotion.FinalIK;
using UniRx;

namespace MyUnityChan {
    public class PlayerDemo : ObjectBase {

        [SerializeField]
        public Const.CharacterName target_player;

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
            pm.switchPlayerCharacter(target_player);
            foreach ( var kv in pm.players ) {
                center_positions.Add(kv.Key, kv.Value.transform.position);
            }
        }

        public void play(Func<UniRx.IObserver<string>, IEnumerator> co) {
            if ( demo != null )
                demo.Dispose();

            pm.controller.clearAllInputs();
            demo = Observable.FromCoroutine<string>(observer => co(observer))
                .Subscribe(action_name => {
                    pm.getNowPlayerComponent().action_manager.forceAction(action_name);
                });
        }

        public void equip(Const.ID.Weapon weapon_id) {
            Player player = pm.getNowPlayerComponent();

            var o = PrefabInstantiater.create(Const.Prefab.Weapons[weapon_id],
                                              transform.position,
                                              Hierarchy.Layout.DAMAGE_OBJECT);
            var weapon = o.GetComponent<Weapon>();
            var slot = weapon.interaction_slot;
            setDemoLayer(o);
            player.equip(weapon, slot);
        }

        public void unequip() {
            DebugManager.log("demo unequip");
            Player player = pm.getNowPlayerComponent();
            InteractionObject unequipped = player.unequip();
            if ( unequipped )
                DebugManager.log("demo destroy");
                Destroy(unequipped.gameObject);
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

        public void setDemoLayer(GameObject obj) {
            obj.setLayer("ModelViewer");
        }
    }
}