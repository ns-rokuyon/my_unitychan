using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerManager : ObjectBase {
        public GameObject default_unitychan;
        public GameObject mini_unitychan;

        public GameObject hpgauge_object_ref;

        public string controller_name;

        private Dictionary<Const.CharacterName, GameObject> switchable_player_characters;
        private Const.CharacterName now;

        public PlayerCamera camera { get; set; }
        public Controller controller { get; set; }
        public HpGauge hpgauge { get; set; }
        public PlayerStatus status { get; set; }

        void Awake() {
            switchable_player_characters = new Dictionary<Const.CharacterName, GameObject>();

            default_unitychan.GetComponent<Player>().manager = this;
            switchable_player_characters.Add(Const.CharacterName.UNITYCHAN, default_unitychan);
            now = Const.CharacterName.UNITYCHAN;
            Player now_player = getNowPlayer().GetComponent<Player>();
            now_player.character_name = now;

            // camera setup
            camera = PrefabInstantiater.createAndGetComponent<PlayerCamera>(Const.Prefab.Camera["PLAYER_CAMERA"], Hierarchy.Layout.CAMERA);
            camera.player_manager = this;

            // player status setup
            status = GetComponent<PlayerStatus>();
            now_player.status = status;

            // controller setup
            controller = PrefabInstantiater.create(Const.Prefab.Controller[controller_name], this.gameObject).GetComponent<Controller>();
            controller.setSelf(now_player);
            now_player.setController(controller);

            // HP gauge setup
            if ( hpgauge_object_ref )
                hpgauge = hpgauge_object_ref.GetComponent<HpGauge>();

            // set player to GameStateManager
            GameStateManager.self().player_manager = this;

        }

        void Start() {
            Player now_player = getNowPlayer().GetComponent<Player>();

            now_player.registerActions(new List<Const.PlayerAction> {
                Const.PlayerAction.ATTACK, Const.PlayerAction.BEAM, Const.PlayerAction.DASH,
                Const.PlayerAction.GUARD, Const.PlayerAction.HADOUKEN, Const.PlayerAction.SLIDING
            });

            hpgauge.setCharacter(now_player);
            hpgauge.setPosition(new Vector3(200, -24, 10));

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
                    // Enable GameObject
                    pair.Value.SetActive(true);

                    // Copy position
                    pair.Value.transform.position = player.gameObject.transform.position;

                    // Switch controller's focus to next player object
                    controller.setSelf(pair.Value.GetComponent<Player>());

                    // Switch player object for hp gauge
                    hpgauge.setCharacter(pair.Value.GetComponent<Player>());

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