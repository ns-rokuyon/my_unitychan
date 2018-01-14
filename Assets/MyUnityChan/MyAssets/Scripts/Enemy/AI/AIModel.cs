using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using System;

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
        public int current_routine { get; set; }

        [ReadOnly]
        public string current_sub_ai_name;

        public abstract AI define();

        public virtual List<SubAI> defineSubAIs(AI main_ai) {
            return new List<SubAI>();
        }

        public virtual Type sub_ai_routine_type {
            get { return null; }
        }

        void Start() {
            if ( sub_ai_routine_type != null ) {
                current_sub_ai_name = getCurrentSubAIName();

                this.ObserveEveryValueChanged(_ => current_routine)
                    .Subscribe(_ => current_sub_ai_name = getCurrentSubAIName())
                    .AddTo(this);
            }
        }

        public virtual void init() {
        }

        public void next_routine(object r) {
            current_routine = (int)r;
        }

        public void next_routine(object r, float p) {
            if ( UnityEngine.Random.Range(0.0f, 1.0f) <= p ) {
                current_routine = (int)r;
            }
        }

        public virtual string getCurrentSubAIName() {
            if ( sub_ai_routine_type == null )
                return "";
            return Enum.ToObject(sub_ai_routine_type, current_routine).ToString();
        }
    }
}