using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class PhysicsObject : ObjectBase {
        [SerializeField] public bool show_debug_window;
        [SerializeField] public PhysicsSpec spec;

        public float m {
            get { return rigid_body.mass; }
        }

        public float v2 {
            get { return rigid_body.velocity.sqrMagnitude; }
        }

        public Vector3 velocity {
            get { return rigid_body.velocity; }
        }

        public float kinetic_energy {
            get { return 0.5f * m * v2; }
        }

        public GameObject debug_window { get; set; }
        public Text velocity_debug_text { get; set; }
        public Text energy_debug_text { get; set; }

        void Awake() {
            setupSoundPlayer();
            spec.self = this;
        }

        void Start() {
            this.UpdateAsObservable()
                .Where(_ => show_debug_window)
                .Subscribe(_ => {
                    if ( !debug_window ) {
                        debug_window = PrefabInstantiater.createWorldUI("Prefabs/UI/World/PhysicsObjectData");
                        velocity_debug_text = debug_window.transform.Find("Velocity").gameObject.GetComponent<Text>();
                        energy_debug_text = debug_window.transform.Find("Energy").gameObject.GetComponent<Text>();
                    }
                    velocity_debug_text.text = velocity.ToString();
                    energy_debug_text.text = kinetic_energy.ToString();
                    debug_window.transform.position = transform.position.add(0, 1.0f, -1.0f);
                });

            this.ObserveEveryValueChanged(_ => show_debug_window)
                .Where(f => !f)
                .Subscribe(_ => {
                    if ( debug_window ) {
                        Destroy(debug_window);
                    }
                });
        }

        public void OnCollisionEnter(Collision collision) {
            switch ( collision.collider.tag ) {
                case "Ground":
                    {
                        spec.playSound(this);
                        spec.playEffect();
                        break;
                    }
                case "Player":
                    {
                        Player player = collision.collider.gameObject.GetComponent<Player>();
                        spec.attack(player);
                        spec.playEffect(player);
                        spec.playSound();
                        break;
                    }
                case "Enemy":
                    {
                        Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();
                        spec.attack(enemy);
                        spec.playEffect(enemy);
                        spec.playSound();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}