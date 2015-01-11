using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Status : ObjectBase {

        void Awake() {
            awake();
        }

        // Use this for initialization
        void Start() {
            start();
        }

        // Update is called once per frame
        void Update() {
            update();
        }

        protected virtual void awake() { }
        protected virtual void start() { }
        protected virtual void update() { }
    }
}
