using UnityEngine;
using System.Collections.Generic;
using System;

namespace MyUnityChan {
    public partial class AI {
        public partial class Def {
            public string name { get; set; }
            public List<Condition> conditions { get; set; }
            public Condition break_condition { get; set; }
            public Behavior true_behavior { get; set; }
            public Behavior false_behavior { get; set; }

            private Def() {
                conditions = new List<Condition>();
            }

            public class Condition {
                public Func<State, bool> cond { get; set; }
                public Condition() { }
                public Condition(Func<State, bool> _cond) {
                    cond = _cond;
                }

                public virtual bool check(State state) {
                    return cond(state);
                }
            }

            public class FrameCondition : Condition {
                public Func<State, FrameCondition, bool> framecond { get; set; }
                public int last_checked { get; set; }
                public int last_true { get; set; }
                public int last_false { get; set; }
                public int now { get { return Time.frameCount; } }

                public FrameCondition(Func<State, FrameCondition, bool> _cond) {
                    framecond = _cond;
                    last_checked = last_false = last_true = 0;
                }

                public override bool check(State state) {
                    bool flag = framecond(state, this);
                    last_checked = Time.frameCount;
                    if ( flag )
                        last_true = Time.frameCount;
                    else
                        last_false = Time.frameCount;
                    return flag;
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

            public Def Required(Func<bool> _cond) {
                if ( _cond() ) {
                    return this;
                }
                throw new Exception();
            }

            public Def If(Func<State, bool> _cond) {
                conditions.Add(new Condition(_cond));
                return this;
            }

            public Def And(Func<State, bool> _cond) {
                return If(_cond);
            }

            public Def Random(float p) {
                conditions.Add(new Condition(_ => UnityEngine.Random.value <= p));
                return this;
            }

            public Def StopIf(Func<State, bool> _cond) {
                conditions.Add(new Condition(_cond));
                true_behavior = new Behavior(s => s.message.stop = true);
                return this;
            }

            public Def Interval(int frame) {
                conditions.Add(new FrameCondition((s, f) => f.now - f.last_true >= frame));
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

            public Def Else(Action<State> _behavior) {
                false_behavior = new Behavior(_behavior);
                return this;
            }

            public Def Keep(Action<State> _behavior) {
                conditions.Add(new Condition(s => true));
                true_behavior = new Behavior(_behavior);
                return this;
            }

            // Deprecated
            public Def RandomThen(Action<State> _behavior, float probability) {
                true_behavior = new ProbBehavior(_behavior, probability);
                return this;
            }
            
        }
    }
}