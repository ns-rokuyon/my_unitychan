using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyUnityChan {
    public class ModelViewCamera : ObjectBase {
        public GameObject target;
        public float distance;
        public Vector3 angle;
        public Vector3 offset;

        void Update() {
            if ( !target ) return;

            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - distance);
            transform.position = transform.position.add(offset.x, offset.y, offset.z);
            gameObject.transform.rotation = Quaternion.Euler(angle);
        }
    }
}
