using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class PlayerManager : ObjectBase {
        public GameObject default_unitychan;
        public GameObject mini_unitychan;

        public enum PlayerCharacterName {
            UNITYCHAN,
            MINI_UNITYCHAN
        }

        private Dictionary<PlayerCharacterName, GameObject> switchable_player_characters;
        private PlayerCharacterName now;

        public PlayerCamera camera { get; set; }

        void Awake() {
            switchable_player_characters = new Dictionary<PlayerCharacterName, GameObject>();

            default_unitychan.GetComponent<Player>().manager = this;
            switchable_player_characters.Add(PlayerCharacterName.UNITYCHAN, default_unitychan);
            now = PlayerCharacterName.UNITYCHAN;

            // camera setup
            camera = PrefabInstantiater.createAndGetComponent<PlayerCamera>(Const.Prefab.Camera["PLAYER_CAMERA"], Hierarchy.Layout.CAMERA);
            camera.player_manager = this;

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
                    pair.Value.SetActive(true);
                    pair.Value.transform.position = player.gameObject.transform.position;
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
            }
        }
    }
}