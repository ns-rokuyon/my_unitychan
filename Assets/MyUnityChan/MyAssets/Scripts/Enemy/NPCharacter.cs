using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MyUnityChan {
    public class NPCharacter : Character {
        static protected List<GameObject> players = new List<GameObject>();
        const int PLAYER_TOUCHING_FRAME_OFFSET = 40;

        protected int player_hit_damage = 3;

        static public void setPlayers() {
            removeNullPlayers();
            foreach ( GameObject pl in GameObject.FindGameObjectsWithTag("Player") ) {
                Debug.Log(pl.name);
                players.Add(pl);
            }
        }

        static public void removeNullPlayers() {
            players.RemoveAll(pl => pl == null);
        }

        static public GameObject findPlayerByName(string name) {
            foreach ( GameObject player in players ) {
                if ( !player.activeSelf ) {
                    continue;
                }
                Player script = player.GetComponent<Player>();
                if ( script.player_name == name ) {
                    return player;
                }
            }
            throw new System.InvalidOperationException("not exists : " + name);
        }

        static public GameObject findNearestPlayer(Vector3 pos) {
            if ( players.Count == 1 ) return players[0];
            return players.Where(player => player.gameObject.activeSelf)
                .OrderByDescending(player => Vector3.Distance(pos, player.transform.position)).First();
        }

        protected Dictionary<string, int> touching_players = new Dictionary<string, int>();

        protected virtual void die() { }

        protected void checkPlayerTouched() {
            if ( isFrozen() || isStunned() || isHitstopping() || isInputLocked() ) return;

            foreach ( KeyValuePair<string, int> pair in touching_players ) {
                if ( pair.Value > PLAYER_TOUCHING_FRAME_OFFSET ) {
                    GameObject player = findPlayerByName(pair.Key);
                    Player script = player.GetComponent<Player>();

                    StartCoroutine(touchPlayer(script));
                }
            }
        }

        protected IEnumerator touchPlayer(Player player) {
            player.damage(player_hit_damage);
            yield return null;
            clearTouchingCount(player);
        }

        protected void faceForward() {
            float dir_x = controller.keyHorizontal();
            gameObject.transform.LookAt(new Vector3(gameObject.transform.position.x + dir_x * 100.0f, gameObject.transform.position.y, transform.position.z));
        }

        public void faceToPlayer() {
            float dir_x = Mathf.Sign(findNearestPlayer(gameObject.transform.position).transform.position.x - gameObject.transform.position.x);
            gameObject.transform.LookAt(new Vector3(gameObject.transform.position.x + dir_x * 100.0f, gameObject.transform.position.y, transform.position.z));
        }

        public override bool isGrounded() {
            if ( !ground_checker )
                return false;
            return ground_checker.isGrounded();
        }

        public float getDistanceXToPlayer() {
            return 0.0f;        // TODO
        }

        public void clearTouchingCount(Player player) {
            if ( touching_players.ContainsKey(player.player_name) )
                touching_players[player.player_name] = 0;
        }

        public void OnCollisionStay(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Player" ) {
                Player player_script = collisionInfo.gameObject.GetComponent<Player>();
                string player_name = player_script.player_name;

                if ( touching_players.ContainsKey(player_name) ) {
                    touching_players[player_name]++;
                }
                else {
                    touching_players[player_name] = 1;
                }
            }
        }

        public void OnCollisionExit(Collision collisionInfo) {
            if ( collisionInfo.gameObject.tag == "Player" ) {
                Player player_script = collisionInfo.gameObject.GetComponent<Player>();
                clearTouchingCount(player_script);
            }
        }
    }
}