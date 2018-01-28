using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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

            // Reset all states on each behavior and condition
            public void reset() {
                if ( true_behavior != null )
                    true_behavior.reset();
                if ( false_behavior != null )
                    false_behavior.reset();
                conditions.ForEach(c => c.reset());
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

                public virtual void reset() {
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
                    reset();
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

            public class TimerCondition : Condition {
                public int max_count { get; private set; }
                public bool done { get; private set; }

                public TimerCondition(int _max_count) {
                    max_count = _max_count;
                }

                public override bool check(State state) {
                    if ( done ) {
                        if ( state.model.current_count < max_count )
                            done = false;
                        else
                            return false;
                    }

                    if ( state.model.current_count >= max_count ) {
                        done = true;
                        return true;
                    }
                    return false;
                }
            }

            public class Behavior {
                public Action<State> behavior { get; private set; }
                public int count { get; protected set; }

                public Behavior(Action<State> _f) {
                    behavior = _f;
                    count = 0;
                }

                public virtual void act(State state) {
                    behavior(state);
                    count++;
                }

                public virtual void reset() {
                    count = 0;
                }
            }

            public class ProbBehavior : Behavior {
                public float p { get; set; }
                public ProbBehavior(Action<State> _f, float prob) : base(_f) {
                    p = prob;
                }

                public override void act(State state) {
                    if ( UnityEngine.Random.value <= p ) {
                        behavior(state);
                        count++;
                    }
                }
            }

            public class ResetableBehavior : Behavior {
                public System.Action reseter { get; private set; }

                public ResetableBehavior(System.Action<State> _f, System.Action _g) : base(_f) {
                    reseter = _g;
                }

                public override void reset() {
                    base.reset();
                    reseter();
                }
            }

            public class ParameterBehavior<T> : Behavior {
                public T apply { get; private set; }
                public T restore { get; private set; }
                public System.Func<T> getter { get; private set; }
                public System.Action<T> setter { get; private set; }

                public ParameterBehavior(Func<T> _getter, Action<T> _setter, T x) : base(_ => { }) {
                    apply = x;
                    getter = _getter;
                    setter = _setter;
                    restore = getter();
                }

                public override void act(State state) {
                    base.act(state);
                    setter(apply);
                }

                public override void reset() {
                    base.reset();
                    setter(restore);
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

            public Def Unless(Func<State, bool> _cond) {
                conditions.Add(new Condition(s => !_cond(s)));
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
                if ( frame <= 0 )
                    return this;
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

            public Def Do(Action<State> _behavior) {
                return Keep(_behavior);
            }

            public Def Empty() {
                return this;
            }

            public Def Once() {
                conditions.Add(new Condition(s => true_behavior.count == 0));
                return this;
            }

            public Def Start(int frame) {
                conditions.Add(new Condition(s => Time.frameCount >= frame));
                return this;
            }

            public Def At(int frame, Action<State> _behavior) {
                conditions.Add(new TimerCondition(frame));
                true_behavior = new Behavior(_behavior);
                return this;
            }

            public Def Parameter<T>(System.Func<T> getter, System.Action<T> setter, T x) {
                conditions.Add(new Condition(s => true));
                true_behavior = new ParameterBehavior<T>(getter, setter, x);
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