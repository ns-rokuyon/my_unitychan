using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerManager : ObjectBase {
        public GameObject default_unitychan;
        public GameObject mini_unitychan;

        public GameObject hpgauge_object_ref;
        public GameObject reserved_hpgauge_object_ref;

        public string controller_name;

        private Dictionary<Const.CharacterName, GameObject> switchable_player_characters;
        private Const.CharacterName now;

        public PlayerCamera camera { get; set; }
        public Controller controller { get; set; }
        public HpGauge hpgauge { get; set; }
        public ReservedHpGauge reserved_hpgauge { get; set; }
        public PlayerStatus status { get; set; }

        void Awake() {
            switchable_player_characters = new Dictionary<Const.CharacterName, GameObject>();

            default_unitychan.GetComponent<Player>().manager = this;
            switchable_player_characters.Add(Const.CharacterName.UNITYCHAN, default_unitychan);
            now = Const.CharacterName.UNITYCHAN;
            Player now_player = getNowPlayer().GetComponent<Player>();
            now_player.character_name = now;

            // camera setup
            GameObject camera_obj = GameObject.Find("Camera/PlayerCamera");
            if ( camera_obj ) {
                camera = camera_obj.GetComponent<PlayerCamera>();
            }
            else {
                camera = PrefabInstantiater.createAndGetComponent<PlayerCamera>(Const.Prefab.Camera["PLAYER_CAMERA"], Hierarchy.Layout.CAMERA);
            }
            camera.player_manager = this;

            // player status setup
            status = GetComponent<PlayerStatus>();
            status.addEnergyTank();
            now_player.status = status;

            // controller setup
            controller = PrefabInstantiater.create(Const.Prefab.Controller[controller_name], this.gameObject).GetComponent<Controller>();
            controller.setSelf(now_player);
            now_player.setController(controller);

            // HP gauge setup
            if ( hpgauge_object_ref )
                hpgauge = hpgauge_object_ref.GetComponent<HpGauge>();

            if ( reserved_hpgauge_object_ref )
                reserved_hpgauge = reserved_hpgauge_object_ref.GetComponent<ReservedHpGauge>();

            // set player to GameStateManager
            GameStateManager.self().player_manager = this;

        }

        void Start() {
            Player now_player = getNowPlayer().GetComponent<Player>();

            now_player.registerActions(new List<Const.PlayerAction> {
                Const.PlayerAction.ATTACK, Const.PlayerAction.BEAM, Const.PlayerAction.DASH, Const.PlayerAction.MISSILE,
                Const.PlayerAction.GUARD, Const.PlayerAction.HADOUKEN, Const.PlayerAction.SLIDING
            });

            hpgauge.setCharacter(now_player);
            reserved_hpgauge.setCharacter(now_player);

            // TODO
            addPlayerCharacter(Const.CharacterName.MINI_UNITYCHAN);
        }

        public GameObject getNowPlayer() {
            return switchable_player_characters[now];
        } 

        public void switchPlayerCharacter(Const.CharacterName name) {
            Player player = switchable_player_characters[now].GetComponent<Player>();
            foreach ( var pair in switchable_player_characters ) {
                if ( pair.Key == name ) {
                    Player next_player = pair.Value.GetComponent<Player>(); 

                    // Enable GameObject
                    pair.Value.SetActive(true);

                    // Copy position
                    pair.Value.transform.position = player.gameObject.transform.position;
                    next_player.last_entrypoint = player.last_entrypoint;

                    // Switch controller's focus to next player object
                    controller.setSelf(next_player);

                    // Switch player object for hp gauge
                    hpgauge.setCharacter(next_player);

                    // Replace character name
                    now = name;
                }
                else {
                    pair.Value.SetActive(false);
                }
            }
        }

        public void switchPlayerCharacter() {
            if ( now == Const.CharacterName.UNITYCHAN ) switchPlayerCharacter(Const.CharacterName.MINI_UNITYCHAN);
            else switchPlayerCharacter(Const.CharacterName.UNITYCHAN);
        }

        public void addPlayerCharacter(Const.CharacterName name) {
            Player new_player = null;
            switch (name) {
                case Const.CharacterName.MINI_UNITYCHAN:
                    if ( mini_unitychan ) {
                        mini_unitychan.SetActive(false);
                        new_player = mini_unitychan.GetComponent<Player>();
                        switchable_player_characters.Add(Const.CharacterName.MINI_UNITYCHAN, mini_unitychan);
                        new_player.character_name = Const.CharacterName.MINI_UNITYCHAN;
                    }
                    break;
                default:
                    break;
            }
            if ( new_player ) {
                new_player.manager = this;
                new_player.status = status;
                new_player.setController(controller);
            }
        }
    }
}