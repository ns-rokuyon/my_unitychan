using UnityEngine;
using System.Collections.Generic;

namespace MyUnityChan {
    public abstract partial class AIModel : ObjectBase {
        /* AIModel class

            Implement define() which returns a main AI instance.

            Implementation of defineSubAI() supports changeable multiple AIs 
            based on routine pairs of id and AI.
            When sub AIs are used, the only main AI is able to switch them.
            AI.think() will be called based on following rules.
                - Main AI : Every frame
                - Sub AI : The only one of them specified by current_routine will be called at every frame

        */
        public bool debug;      // Debug mode

        public AIController controller { get; set; }
        public Enemy self { get; set; }

        [ReadOnly]
        public int current_routine = 0;

        public abstract AI define();

        public virtual Dictionary<int, AI> defineSubAI() {
            return new Dictionary<int, AI>();
        }

        public virtual void init() {
        }

        public void next_routine(object r) {
            current_routine = (int)r;
        }
    }
}