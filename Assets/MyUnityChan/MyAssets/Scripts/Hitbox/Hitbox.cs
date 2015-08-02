using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Hitbox : PoolObjectBase {
        public static readonly bool RENDER_HITBOX = true;

        protected bool use_objectpool = false;
        protected string resource_path = null;
        protected int time = 0;                         // active time
        protected TimerState end_timer = null;
        public Vector3 forward { get; set; }

        void Awake() {
            end_timer = new FrameTimerState();
            if ( !RENDER_HITBOX ) {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
            CommonUpdate();
            UniqueUpdate();
        }

        protected virtual void UniqueUpdate() {
        }

        private void CommonUpdate() {
            if ( end_timer == null ) {
                end_timer.createTimer(time);
            }
            else if ( end_timer.isFinished() ) {
                if ( use_objectpool ) {
                    ObjectPoolManager.releaseGameObject(this.gameObject, resource_path);
                }
                else {
                    Destroy(this.gameObject);
                }
                return;
            }
        }

        public void OnTriggerEnter(Collider other) {
            if ( other.tag == "Enemy" ) {
                Debug.Log("hit");
            }
        }

        public virtual void enablePool(string _resource_path) {
            resource_path = _resource_path;
            use_objectpool = true;
        }

        // hitbox depends on other object
        public virtual void ready(GameObject obj, AttackSpec spec) { }

        // static hitbox
        public virtual void ready(Vector3 pos, Vector3 fw, Vector3 offset, AttackSpec spec) { }


        // PoolObject callbacks
        public override void initialize() {
            end_timer = new FrameTimerState();
        }
        public override void finalize() {
            end_timer = null;            
        }
    }
}
