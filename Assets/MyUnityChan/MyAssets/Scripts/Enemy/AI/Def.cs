using UnityEngine;
using System.Collections;
using System;

namespace MyUnityChan {
    public partial class AI {
        public partial class Def {
            public string name { get; set; }
            public Condition condition { get; set; }
            public Condition break_condition { get; set; }
            public Behavior true_behavior { get; set; }
            public Behavior false_behavior { get; set; }

            private Def() {
            }

            public class Condition {
                private Func<State, bool> cond;
                public Condition(Func<State, bool> _cond) {
                    cond = _cond;
                }

                public bool check(State state) {
                    return cond(state);
                }
            }

            public class Behavior {
                public Action<State> behavior { get; private set; }
                public Behavior(Action<State> _f) {
                    behavior = _f;
                }

                public virtual void act(State state) {
                    behavior(state);
                }
            }

            public class ProbBehavior : Behavior {
                public float p { get; set; }
                public ProbBehavior(Action<State> _f, float prob) : base(_f) {
                    p = prob;
                }

                public override void act(State state) {
                    if ( UnityEngine.Random.value <= p )
                        behavior(state);
                }
            }

            public static Def Name(string _name) {
                Def def = new Def();
                def.name = _name;
                return def;
            }

            public Def If(Func<State, bool> _cond) {
                condition = new Condition(_cond);
                return this;
            }

            public Def Random(float p) {
                condition = new Condition(_ => UnityEngine.Random.value <= p);
                return this;
            }

            public Def StopIf(Func<State, bool> _cond) {
                condition = new Condition(_cond);
                true_behavior = new Behavior(s => s.message.stop = true);
                return this;
            }

            public Def Break() {
                if ( false_behavior != null ) {
                    break_condition = new Condition(s => !s.flagstack[s.flagstack.Count - 1]);
                    return this;
                }
                if ( true_behavior != null ) {
                    break_condition = new Condition(s => s.flagstack[s.flagstack.Count - 1]);
                    return this;
                }
                break_condition = new Condition(_ => true);
                return this;
            }

            public Def Then(Action<State> _behavior) {
                true_behavior = new Behavior(_behavior);
                return this;
            }

            public Def RandomThen(Action<State> _behavior, float probability) {
                true_behavior = new ProbBehavior(_behavior, probability);
                return this;
            }
            
            public Def Else(Action<State> _behavior) {
                false_behavior = new Behavior(_behavior);
                return this;
            }

            public Def Keep(Action<State> _behavior) {
                condition = new Condition(s => true);
                true_behavior = new Behavior(_behavior);
                return this;
            }

        }
    }
}