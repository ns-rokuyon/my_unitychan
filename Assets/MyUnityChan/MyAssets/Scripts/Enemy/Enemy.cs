using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public class NPCharacter : Character {
        static protected List<GameObject> players = new List<GameObject>();
        const int PLAYER_TOUCHING_FRAME_OFFSET = 10;

        static public void setPlayers() {
            removeNullPlayers();
            foreach ( GameObject pl in GameObject.FindGameObjectsWithTag("Player") ) {
                players.Add(pl);
            }
        }

        static public void removeNullPlayers() {
            players.RemoveAll(pl => pl == null);
        }

        static public GameObject findPlayerByName(string name) {
            foreach ( GameObject player in players ) {
                Player script = player.GetComponent<Player>();
                if ( script.player_name == name ) {
                    return player;
                }
            }
            throw new System.InvalidOperationException("not exists : " + name);
        }

        static public GameObject findNearestPlayer(Vector3 pos) {
            if ( players.Count == 1 ) {
                return players[0];
            }

            GameObject nearest = null;
            float min_dist = 100000.0f;
            foreach ( GameObject player in players ) {
                if ( nearest == null ) {
                    nearest = player;
                    continue;
                }

                float tmp_dist = Vector3.Distance(nearest.transform.position, player.transform.position);
                if ( tmp_dist < min_dist ) {
                    min_dist = tmp_dist;
                    nearest = player;
                }
            }
            return nearest;
        }

        protected Dictionary<string, int> touching_players = new Dictionary<string, int>();

        protected virtual void start() { }
        protected virtual void update() { }
        protected virtual void die() { }

        protected void checkPlayerTouched() {
            foreach ( KeyValuePair<string, int> pair in touching_players ) {
                if ( pair.Value > PLAYER_TOUCHING_FRAME_OFFSET ) {
                    GameObject player = findPlayerByName(pair.Key);
                    Player script = player.GetComponent<Player>();

                    script.damage();
                }
            }
        }

        protected void faceForward() {
            float dir_x = controller.keyHorizontal();
            gameObject.transform.LookAt(new Vector3(gameObject.transform.position.x + dir_x * 100.0f, gameObject.transform.position.y, transform.position.z));
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
                touching_players[player_script.player_name] = 0;
            }
        }
    }

    public abstract class Enemy : NPCharacter {
        public GameObject controller_prefab;
        public GameObject enemy_action_manager_prefab;

        protected int max_hp;

        protected int stunned = 0;
        protected EnemyActionManager action_manager;

        protected void loadAttachedAI() {

            GameObject controller_inst = Instantiate(controller_prefab) as GameObject;
            controller_inst.setParent(gameObject);
            controller = controller_inst.GetComponent<Controller>();

            ((AIController)controller).setSelf(this);
        }

        public void stun(int stun_power) {
            stunned = stun_power;
        }

        public void damage(int dam) {
            if ( !status.invincible.now() ) {
                status.invincible.enable(30);
                status.hp -= dam;
            }
        }

        public bool isStunned() {
            return stunned > 0 ? true : false;
        }

        protected void updateStunned() {
            if ( stunned > 0 ) {
                stunned--;
            }
        }

        public virtual void spawn() {
            touching_players.Clear();
            setHP(max_hp);
            this.gameObject.SetActive(true);
        }


        // Use this for initialization
        void Start() {
            loadAttachedAI();
            action_manager = new EnemyActionManager();
            inputlock_timer = new FrameTimerState();

            // enemy status setup
            status = (Instantiate(status_prefab) as GameObject).setParent(gameObject).GetComponent<EnemyStatus>();

            start();
        }

        // Update is called once per frame
        void Update() {
            updateStunned();
            checkPlayerTouched();
            faceForward();

            update();
        }


    }
}
