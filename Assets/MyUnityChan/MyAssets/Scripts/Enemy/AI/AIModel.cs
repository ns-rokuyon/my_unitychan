using UnityEngine;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

namespace MyUnityChan {
    public abstract partial class AIModel : ObjectBase {
        /* AIModel class

            The method define() should set multiple AIs to ai_table with registerRoutine() method.
            The ai_table keys are corresponding to each changeable AI.
        */
        public bool debug;      // Debug mode
        public bool stop;

        public AIDebugger debugger { get; set; }

        public IDisposable brain { get; set; }
        public AIController controller { get; set; }
        public Enemy self { get; set; }

        public Dictionary<int, AI> ai_table { get; set; }

        // Current routine id
        public int current_routine { get; private set; }

        // Current active AI object
        public AI ai {
            get { return ai_table[current_routine]; }
        }

        public virtual Type routine_type {
            get { return null; }
        }

        [ReadOnly]
        public string current_routine_name;

        [ReadOnly]
        public int current_count;

        public abstract void define();
        
        void Awake() {
            ai_table = new Dictionary<int, AI>();
            self = GetComponent<Enemy>();
            debugger = GetComponent<AIDebugger>();
        }

        void Start() {
            if ( routine_type != null ) {
                current_routine_name = getCurrentRoutineName();

                // Sync current_sub_ai_name for inspector
                this.ObserveEveryValueChanged(_ => current_routine)
                    .Subscribe(_ => current_routine_name = getCurrentRoutineName())
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
                        .Subscribe(_ => {
                            current_count++;
                            ai.think(controller.getObservedState());
                        })
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
            ai_table.Add((int)i, ai);
        }

        public void next_routine(object r) {
            if ( debugger )
                debugger.pushLog("next_routine: " + getCurrentRoutineName() + " -> " + r.ToString());

            current_routine = (int)r;
            current_count = 0;

            ai_table.Values.ToList().ForEach(_ai => _ai.reset());
        }

        public void next_routine(object r, float p) {
            if ( UnityEngine.Random.Range(0.0f, 1.0f) <= p ) {
                next_routine(r);
            }
        }

        public void freeze(int frame) {
            stop = true;
            delay("AIModel.freeze", frame, () => {
                stop = false;
            });
        }

        public string getCurrentRoutineName() {
            if ( routine_type == null )
                return "";
            return Enum.ToObject(routine_type, current_routine).ToString();
        }
    }
}