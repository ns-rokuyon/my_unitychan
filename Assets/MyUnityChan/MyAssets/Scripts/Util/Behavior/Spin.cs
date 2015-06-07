using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class Spin : ObjectBase {
        public Vector3 angle;

        // Update is called once per frame
        void Update() {
            transform.Rotate(angle * Time.deltaTime);
        }
    }
}
