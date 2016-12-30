using UnityEngine;
using System.Collections;
using UniRx;

namespace MyUnityChan {
    public class Hitbox : PoolObjectBase {
        public static bool RENDER_HITBOX = true;

        public GameObject owner = null;
        public bool persistent;

        protected bool use_objectpool = false;
        protected string resource_path = null;
        protected int time = 0;                         // active time
        public Vector3 forward { get; set; }

        void Awake() {
            if ( !RENDER_HITBOX ) {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        protected virtual void UniqueUpdate() {
        }

        private void CommonUpdate() {
        }

        public void startCountdown(int frame) {
            Observable.EveryUpdate()
                .Subscribe(_ => {
                    CommonUpdate();
                    UniqueUpdate();
                });

            Observable.TimerFrame(frame)
                .Subscribe(_ => destroy());
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

        public void setEnabledCollider(bool f) {
            GetComponent<Collider>().enabled = f;
        }

        protected void destroy() {
            //end_timer.destroy();

            if ( persistent ) {
                setEnabledCollider(false);
                return;
            }

            //end_timer = null;
            if ( use_objectpool ) {
                ObjectPoolManager.releaseGameObject(this.gameObject, resource_path);
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
