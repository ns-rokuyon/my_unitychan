using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Hitbox : PoolObjectBase {
        public static bool RENDER_HITBOX = true;

        public GameObject owner = null;
        public bool persistent;
        public bool continuous_hit = false;     // Required persistent = true
        public int continuous_hit_interval_frame = 0;

        protected bool use_objectpool = false;
        protected string resource_path = null;
        protected int time = 0;                         // active time
        public Vector3 forward { get; set; }
        public System.Action<Hitbox> position_updater { get; set; }

        void Awake() {
            if ( !RENDER_HITBOX ) {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if ( LayerMask.LayerToName(gameObject.layer) == "DamageObject" ) {
                DebugManager.error(
                    "This hitbox will not be triggered by any characters because hitbox is set DamageObject layer",
                    gameObject);
            }
        }

        protected virtual void UniqueUpdate() {
        }

        private void CommonUpdate() {
            if ( position_updater == null )
                return;
            position_updater(this);
        }

        public void startCountdown(int frame) {
            Observable.EveryUpdate()
                .Subscribe(_ => {
                    CommonUpdate();
                    UniqueUpdate();
                })
                .AddTo(this);

            if ( frame > 0 )
                Observable.TimerFrame(frame)
                    .Subscribe(_ => destroy())
                    .AddTo(this);
        }

        public virtual void OnTriggerEnter(Collider other) {
        }

        public virtual void OnTriggerStay(Collider other) {
        }

        public virtual void enablePool(string _resource_path) {
            resource_path = _resource_path;
            use_objectpool = true;
        }

        // hitbox depends on other object
        public virtual void ready(GameObject obj, AttackSpec spec, bool keep_position = false) { }

        // static hitbox
        public virtual void ready(Vector3 pos, Vector3 fw, Vector3 offset, AttackSpec spec) { }

        public void setOwner(GameObject go) {
            owner = go;
        }

        public GameObject getOwner() {
            return owner;
        }

        public T getOwner<T>() {
            if ( !owner )
                return default(T);
            return owner.GetComponent<T>();
        }

        public bool isOwner(Character character) {
            return isOwner(character.gameObject);
        }

        public bool isOwner(GameObject obj) {
            if ( owner == null ) return false;
            if ( owner == obj ) return true;
            return false;
        }

        public void rescale(float r) {
            transform.localScale = Vector3.one * r;
        }

        public void rescale(Vector3 s) {
            transform.localScale = s;
        }

        public void setEnabledCollider(bool f) {
            GetComponent<Collider>().enabled = f;
        }

        protected void destroy() {
            if ( persistent ) {
                setEnabledCollider(false);
                return;
            }

            if ( use_objectpool ) {
                ObjectPoolManager.releaseGameObject(this);
            }
            else {
                Destroy(this.gameObject);
            }
            return;
        }

        // PoolObject callbacks
        public override void initialize() {
        }

        public override void finalize() {
        }
    }
}
