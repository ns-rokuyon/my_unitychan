using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerManager : ObjectBase {
        public GameObject default_unitychan;
        public GameObject mini_unitychan;

        public string controller_name;

        public enum PlayerCharacterName {
            UNITYCHAN,
            MINI_UNITYCHAN
        }

        private Dictionary<PlayerCharacterName, GameObject> switchable_player_characters;
        private PlayerCharacterName now;

        public PlayerCamera camera { get; set; }
        public Controller controller { get; set; }
        public HpGauge hpgauge { get; set; }
        public PlayerStatus status { get; set; }

        void Awake() {
            switchable_player_characters = new Dictionary<PlayerCharacterName, GameObject>();

            default_unitychan.GetComponent<Player>().manager = this;
            switchable_player_characters.Add(PlayerCharacterName.UNITYCHAN, default_unitychan);
            now = PlayerCharacterName.UNITYCHAN;
            Player now_player = getNowPlayer().GetComponent<Player>();

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
            hpgauge = PrefabInstantiater.create(Const.Prefab.UI["PLAYER_HP_GAUGE"], HpGauge.getCanvas("Canvas")).GetComponent<HpGauge>();
            hpgauge.setCharacter(now_player);
            hpgauge.setPosition(new Vector3(200, -24, 10));
            //hpgauge.transform.SetParent(HpGauge.getCanvas().transform, false);

            // set player to GameStateManager
            GameStateManager.self().player_manager = this;

            // TODO
            addPlayerCharacter(PlayerCharacterName.MINI_UNITYCHAN);
        }

        public GameObject getNowPlayer() {
            return switchable_player_characters[now];
        } 

        public void switchPlayerCharacter(PlayerCharacterName name) {
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
            if ( now == PlayerCharacterName.UNITYCHAN ) switchPlayerCharacter(PlayerCharacterName.MINI_UNITYCHAN);
            else switchPlayerCharacter(PlayerCharacterName.UNITYCHAN);
        }

        public void addPlayerCharacter(PlayerCharacterName name) {
            Player new_player = null;
            switch (name) {
                case PlayerCharacterName.MINI_UNITYCHAN:
                    if ( mini_unitychan ) {
                        mini_unitychan.SetActive(false);
                        new_player = mini_unitychan.GetComponent<Player>();
                        switchable_player_characters.Add(PlayerCharacterName.MINI_UNITYCHAN, mini_unitychan);
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