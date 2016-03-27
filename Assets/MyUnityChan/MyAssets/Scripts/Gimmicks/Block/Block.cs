using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class Block : ObjectBase {
        public virtual void damage(int dam) { }
    }
}
