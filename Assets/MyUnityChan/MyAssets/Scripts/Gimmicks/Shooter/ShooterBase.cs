using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public abstract class ShooterBase : ObjectBase {
        public bool auto = false;           // Auto trigger in each shooting frame
        public bool auto_aim = false;       // Use auto aimer
        [SerializeField] public Const.ID.AimType auto_aim_type = Const.ID.AimType._NO;
        public Vector3 base_direction = new Vector3(1.0f, 0.0f, 0.0f);
        public Vector3 muzzle_offset = Vector3.zero;

        [ReadOnly] public bool shooting;
        protected SoundPlayer sound_player;

        /*
           [ round=1 ]
            |-----interval_frame-----|-------------- -- -
          shoot                    shoot

           [ round=3 ]
            |--burst_delta--|--burst_delta--|-----interval_frame-----|--burst_delta--|-- -
          shoot           shoot           shoot                    shoot           shoot
        */
        protected int n_round_burst;
        protected int burst_delta_frame;
        protected int interval_frame;
        protected string hitbox_name;
        protected Const.ID.SE se_name;


        // Angle of shooting
        public virtual Vector3 direction {
            get { return base_direction; }
        }

        public bool triggered { get; protected set; }     // Flag for specifying shooting frame manually
        public Character owner { get; protected set; }

        public virtual void Start() {
            shooting = false;
            owner = GetComponent<Character>();

            // Auto aimer
            this.UpdateAsObservable()
                .Where(_ => auto_aim)
                .Subscribe(_ => {
                    switch ( auto_aim_type ) {
                        case Const.ID.AimType.FORWARD:
                            {
                                aimForward();
                                break;
                            }
                        case Const.ID.AimType.TO_PLAYER:
                            {
                                aimTo(GameStateManager.getPlayer().transform.position);
                                break;
                            }
                        default:
                            break;
                    }
                });
        }

        public void aimTo(Vector3 pos) {
            float dx = pos.x - transform.position.x;
            float dy = pos.y - transform.position.y;
            float rad = Mathf.Atan2(dy, dx);
            base_direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), base_direction.z);
        }

        public void aimForward() {
            if ( transform.forward.x >= 0 )
                base_direction = new Vector3(1.0f, 0.0f, 0.0f);
            else
                base_direction = new Vector3(-1.0f, 0.0f, 0.0f);
        }

        public abstract void shoot();

        public virtual void sound() {
            if ( se_name == Const.ID.SE._NO ) {
                return;
            }
            if ( sound_player == null ) {
                sound_player = gameObject.GetComponent<SoundPlayer>();
            }
            if ( sound_player == null ) {
                return;
            }
            sound_player.play(se_name);
        }

        public void trigger(bool t=true) {
            triggered = t;
        }
    }
}
