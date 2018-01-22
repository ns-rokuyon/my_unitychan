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
        public bool stop;

        public AIDebugger debugger { get; set; }

        public IDisposable brain { get; set; }
        public AIController controller { get; set; }
        public Enemy self { get; set; }
        public int current_routine { get; set; }

        public Dictionary<int, AI> ai_table { get; set; }

        // Current active AI object
        public AI ai {
            get { return ai_table[current_routine]; }
        }

        public virtual Type sub_ai_routine_type {
            get { return null; }
        }

        [ReadOnly]
        public string current_sub_ai_name;

        public abstract void define();
        
        void Awake() {
            ai_table = new Dictionary<int, AI>();
            self = GetComponent<Enemy>();
            debugger = GetComponent<AIDebugger>();
        }

        void Start() {
            if ( sub_ai_routine_type != null ) {
                current_sub_ai_name = getCurrentSubAIName();

                // Sync current_sub_ai_name for inspector
                this.ObserveEveryValueChanged(_ => current_routine)
                    .Subscribe(_ => current_sub_ai_name = getCurrentSubAIName())
                    .AddTo(this);
            }
        }

        public virtual void init() {
        }

        public void build() {
            kill();

            if ( ai_table.Count == 0 )
                define();

            brain = this.UpdateAsObservable()
                        .Where(_ => controller != null && !controller.isStopped)
                        .Where(_ => gameObject.activeSelf)
                        .Where(_ => !stop)
                        .Where(_ => !TimelineManager.isPlaying)
                        .Subscribe(_ => ai.think(controller.getObservedState()))
                        .AddTo(this);
        }

        public void kill() {
            if ( brain == null )
                return;
            brain.Dispose();
            brain = null;
        }

        public void registerRoutine(AI ai) {
            int i = ai_table.Count;
            registerRoutine(i, ai);
        }

        public void registerRoutine(object i, AI ai) {
            DebugManager.warn("i=" + (int)i);
            ai_table.Add((int)i, ai);
        }

        public void next_routine(object r) {
            debugger.pushLog("next_routine: " + getCurrentSubAIName() + " -> " + r.ToString());
            current_routine = (int)r;
        }

        public void next_routine(object r, float p) {
            if ( UnityEngine.Random.Range(0.0f, 1.0f) <= p ) {
                current_routine = (int)r;
            }
        }

        public void freeze(int frame) {
            stop = true;
            delay("AIModel.freeze", frame, () => {
                stop = false;
            });
        }

        public virtual string getCurrentSubAIName() {
            if ( sub_ai_routine_type == null )
                return "";
            return Enum.ToObject(sub_ai_routine_type, current_routine).ToString();
        }
    }
}