using UnityEngine;
using System.Collections.Generic;
using System;
using UniRx;
using UniRx.Triggers;

namespace MyUnityChan {
    public class AI : StructBase {
        public Character self { get; set; }
        public AIController controller { get; set; }
        public List<Def> patterns { get; set; }
        public IDisposable brain { get; set; }
        public bool debug { get; set; }     // No used

        private AI(Character ch, AIController cont) {
            self = ch;
            controller = cont;
            patterns = new List<Def>();
        }

        public static AI root(Character _self, AIController _controller) {
            AI ai = new AI(_self, _controller);
            // Add common patterns
             ai.def(AI.Def.Name("Area checker")
                   .StopIf(s => ai.self.getAreaName() != s.player.getAreaName()));
            return ai;
        }

        public AI def(Def def) {
            patterns.Add(def);
            return this;
        }

        public AI build() {
            brain = Observable.EveryUpdate()
                .Where(_ => controller != null)
                .Where(_ => !controller.isStopped)
                .Subscribe(_ => think(controller.getObservedState()))
                .AddTo(controller);
            return this;
        }

        public AI kill() {
            brain.Dispose();
            brain = null;
            return this;
        }

        private void think(State state) {
            foreach ( var def in patterns ) {
                bool flag = false;
                if ( def.condition != null && def.condition.check(state) ) {
                    flag = true;
                }
                if ( flag ) {
                    if ( def.true_behavior != null )
                        def.true_behavior.act(state);
                }
                else {
                    if ( def.false_behavior != null )
                        def.false_behavior.act(state);
                }

                if ( state.message.stop ) {
                    // If message.stop in state was changed to true,
                    // stop thinking loop
                    break;
                }
            }
        }

        public class State {
            public Player player { get; set; }
            public Message message { get; set; }

            public State() {
                message = new Message();
            }
        }

        public class Message {
            public bool stop { get; set; }
        }

        public class Def {
            public string name { get; set; }
            public Condition condition { get; set; }
            public Behavior true_behavior { get; set; }
            public Behavior false_behavior { get; set; }

            private Def() { }

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

                public void act(State state) {
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

            public Def StopIf(Func<State, bool> _cond) {
                condition = new Condition(_cond);
                true_behavior = new Behavior(s => s.message.stop = true);
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
                condition = new Condition(s => true);
                true_behavior = new Behavior(_behavior);
                return this;
            }

        }
    }
}