using UnityEngine;

namespace MyUnityChan {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public class ConvexMesh : ObjectBase {
        public GameObject target_object;
    }
}
