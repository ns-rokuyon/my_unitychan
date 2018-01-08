using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;


namespace MyUnityChan {
    public class PlayerManager : ObjectBase {
        public GameObject default_unitychan;
        public GameObject mini_unitychan;

        public GameObject hpgauge_object_ref;
        public GameObject reserved_hpgauge_object_ref;

        [SerializeField]
        public Const.ID.Controller controller_name;

        public new PlayerCamera camera;
        public bool playable;
        public string player_name;

        private Dictionary<Const.CharacterName, GameObject> switchable_player_characters;

        public Const.CharacterName now { get; protected set; }
        public Controller controller { get; set; }
        public HpGauge hpgauge { get; set; }
        public ReservedHpGauge reserved_hpgauge { get; set; }
        public IPassable current_passing { get; set; }
        public PlayerStatus status { get; set; }
        public string area_name { get; set; }
        public Dictionary<Const.CharacterName, GameObject> players {
            get { return switchable_player_characters; }
        }
        public bool gameover {
            get {
                return status.gameover;
            }
            set {
                status.gameover = value;
            }
        }

        void Awake() {
            now = Const.CharacterName._NO;
            switchable_player_characters = new Dictionary<Const.CharacterName, GameObject>();

            // player status setup
            status = GetComponent<PlayerStatus>();
            status.manager = this;
            status.setupAbilities();

            // controller setup
            controller = PrefabInstantiater.create(prefabPath(controller_name), this.gameObject).GetComponent<Controller>();

            // Setup player characters
            addPlayerCharacter(Const.CharacterName.UNITYCHAN);
            addPlayerCharacter(Const.CharacterName.MINI_UNITYCHAN);

            if ( playable ) {
                // camera setup
                camera.player_manager = this;

                // HP gauge setup
                if ( hpgauge_object_ref )
                    hpgauge = hpgauge_object_ref.GetComponent<HpGauge>();

                if ( reserved_hpgauge_object_ref )
                    reserved_hpgauge = reserved_hpgauge_object_ref.GetComponent<ReservedHpGauge>();

                // set player to GameStateManager
                GameStateManager.self().player_manager = this;
            }
        }

        void Start() {
            // Activate
            switchPlayerCharacter(Const.CharacterName.UNITYCHAN);
            Player now_player = getNowPlayer().GetComponent<Player>();

            now_player.registerActions(Const.PlayerDefaultActions[now_player.character_name]);

            if ( playable ) {
                hpgauge.setCharacter(now_player);
                reserved_hpgauge.setCharacter(now_player);

                this.UpdateAsObservable()
                    .Where(_ => current_passing != null)
                    .Subscribe(_ => {
                        if ( !current_passing.passing )
                            current_passing = null;
                    })
                    .AddTo(this);
            }
        }

        public GameObject getNowPlayer() {
            return switchable_player_characters[now];
        } 

        public Player getNowPlayerComponent() {
            return getNowPlayer().GetComponent<Player>();
        }

        public Player getPlayer(Const.CharacterName name) {
            bool debug = false;
            if ( debug ) {
                DebugManager.log("chname=" + name + ", playername=" + player_name);
                foreach ( var key in switchable_player_characters.Keys ) {
                    DebugManager.log("chkey=" + key);
                }
            }
            return switchable_player_characters[name].GetComponent<Player>();
        }

        public void switchPlayerCharacter(Const.CharacterName name) {
            Player player = null;
            if ( now != Const.CharacterName._NO && now != name )
                player = switchable_player_characters[now].GetComponent<Player>();

            foreach ( var pair in switchable_player_characters ) {
                if ( pair.Key == name ) {
                    // Enable GameObject
                    pair.Value.SetActive(true);

                    Player next_player = pair.Value.GetComponent<Player>();
                    DebugManager.log("Player = " + next_player);

                    // Copy position
                    if ( player ) {
                        pair.Value.transform.position = player.transform.position;
                        pair.Value.transform.rotation = player.transform.rotation;
                        next_player.last_entrypoint = player.last_entrypoint;
                    }

                    // Switch controller's focus to next player object
                    controller.setSelf(next_player);

                    // Replace character name
                    now = name;

                    if ( playable ) {
                        // Switch camera's target
                        camera.onTransformPlayer(next_player);

                        // Switch player object for hp gauge
                        hpgauge.setCharacter(next_player);
                    }
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
            if ( switchable_player_characters.ContainsKey(name) )
                return;

            Player new_player = null;
            switch (name) {
                case Const.CharacterName.UNITYCHAN:
                    {
                        if ( default_unitychan ) {
                            if ( default_unitychan.activeSelf )
                                now = Const.CharacterName.UNITYCHAN;
                            default_unitychan.SetActive(true);
                            new_player = default_unitychan.GetComponent<Player>();
                            new_player.character_name = Const.CharacterName.UNITYCHAN;
                            switchable_player_characters.Add(Const.CharacterName.UNITYCHAN, default_unitychan);
                        }
                        break;
                    }
                case Const.CharacterName.MINI_UNITYCHAN:
                    {
                        if ( mini_unitychan ) {
                            if ( mini_unitychan.activeSelf )
                                now = Const.CharacterName.MINI_UNITYCHAN;
                            mini_unitychan.SetActive(false);
                            new_player = mini_unitychan.GetComponent<Player>();
                            new_player.character_name = Const.CharacterName.MINI_UNITYCHAN;
                            switchable_player_characters.Add(Const.CharacterName.MINI_UNITYCHAN, mini_unitychan);
                        }
                        break;
                    }
                default:
                    break;
            }
            if ( new_player ) {
                initPlayer(new_player);
            }
        }

        public void initPlayer(Player player) {
            player.manager = this;
            player.status = status;
            player.setController(controller);
            player.action_manager = player.gameObject.GetComponent<PlayerActionManager>();
        }

        public void lockInput(int frame = 0) {
            players.Values.ToList().ForEach(obj => {
                obj.GetComponent<Player>().lockInput(frame);
            });
        }
    }
}