using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public abstract class AIModel : ObjectBase {
        public bool debug;      // Debug mode

        public AIController controller { get; set; }
        public Enemy self { get; set; }

        public abstract AI define();

        public virtual void init() {
        }
    }
}